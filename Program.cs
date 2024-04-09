using System.Text.Json;
using System.Text.Json.Serialization;
using ai_poker_coach.Models.DataTransferObjects;
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
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddHttpClient();

var app = builder.Build();

int port;

if (app.Environment.IsDevelopment())
{
    port = 5159;

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    port = 5000;
}

app.MapIdentityApi<ApplicationUser>();

app.MapPost(
        "/customLogin",
        async Task<IResult> (LoginRequestDto requestBody, UserManager<ApplicationUser> userManager) =>
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

                return TypedResults.Ok(new LoginResponseDto(user, responseObject!));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return TypedResults.Unauthorized();
                }
                else
                {
                    return TypedResults.StatusCode(Convert.ToInt32(ex.StatusCode));
                }
            }
        }
    )
    .AllowAnonymous();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
