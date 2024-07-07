using System.Text.Json;
using System.Text.Json.Serialization;
using ai_poker_coach.Models.DataTransferObjects;
using ai_poker_coach.Models.DataTransferObjects.Authentication;
using ai_poker_coach.Models.Domain;
using DotNet8Authentication.Data;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Env.Load();
}
else if (builder.Environment.IsProduction())
{
    Env.Load("/home/will/ai-poker-coach/.env");
}

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "oauth2",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        }
    );

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<IdentityDataContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION"));
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<IdentityDataContext>();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DevCorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

int port;

if (app.Environment.IsDevelopment())
{
    port = 5159;

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCorsPolicy");
}
else
{
    port = 5000;
}

app.MapIdentityApi<ApplicationUser>();

app.MapPost(
        "/customLogin",
        async Task<IResult> (AuthRequestDto requestBody, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(requestBody.Email);
            if (user == null)
            {
                return TypedResults.Unauthorized();
            }

            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsJsonAsync($"http://localhost:{port}/login", requestBody);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<LoginInnerResponseDto>(responseBody);

                if (responseObject == null)
                {
                    return TypedResults.UnprocessableEntity("Login error: response object is null");
                }

                return TypedResults.Ok(new LoginResponseDto(user, responseObject));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.UnprocessableEntity($"Login error: {ex.StatusCode} - {ex.Message}");
                }
            }
        }
    )
    .AllowAnonymous();

app.MapPost(
        "/customRegister",
        async Task<IResult> (AuthRequestDto requestBody, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(requestBody.Email);
            if (user != null)
            {
                return TypedResults.Conflict("Email already exists");
            }

            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsJsonAsync($"http://localhost:{port}/register", requestBody);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<RegisterInnerResponseDto>(responseBody);
                    return TypedResults.BadRequest(responseObject);
                }
            }
            catch (HttpRequestException ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }

            try
            {
                var newUser = await userManager.FindByEmailAsync(requestBody.Email);

                if (newUser == null)
                {
                    return TypedResults.UnprocessableEntity("User not found after registration");
                }

                var loginResponse = await httpClient.PostAsJsonAsync($"http://localhost:{port}/login", requestBody);
                loginResponse.EnsureSuccessStatusCode();

                string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
                var loginResponseObject = JsonSerializer.Deserialize<LoginInnerResponseDto>(loginResponseBody);

                if (loginResponseObject == null)
                {
                    return TypedResults.UnprocessableEntity("Login error: response object is null");
                }

                return TypedResults.Ok(new LoginResponseDto(newUser, loginResponseObject));
            }
            catch (HttpRequestException ex)
            {
                return TypedResults.UnprocessableEntity(
                    $"Account successfully created, but there was a problem logging in: {ex.StatusCode} - {ex.Message}"
                );
            }
        }
    )
    .AllowAnonymous();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
