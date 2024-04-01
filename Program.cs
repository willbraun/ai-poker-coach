using System.Text.Json.Serialization;
using ai_poker_coach.Models.Domain;
using DotNet8Authentication.Data;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}
else if (builder.Environment.IsProduction())
{
    DotNetEnv.Env.Load("/home/will/ai-poker-coach/.env");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
