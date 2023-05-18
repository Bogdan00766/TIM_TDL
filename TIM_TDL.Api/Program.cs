using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TIM_TDL.Application.Dtos.Job;
using System.Text;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Application.IServices;
using TIM_TDL.Application.Services;
using TIM_TDL.Application.Utilities;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Infrastructure;
using TIM_TDL.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Sprawdzanie wydawcy tokenu
        ValidateAudience = true, // Sprawdzanie odbiorcy tokenu
        ValidateLifetime = true, // Sprawdzanie wa¿noœci tokenu
        ValidateIssuerSigningKey = true, // Sprawdzanie klucza uwierzytelniania

        ValidIssuer = "issuer", // Poprawny wydawca tokenu
        ValidAudience = "audience", // Poprawny odbiorca tokenu
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("klucz uwierzytelniania")) // Klucz uwierzytelniania
    };
});


builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<TDLDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:TIMConnection"]));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/register", async (RegisterLoginUserDto dto, IUserService userService) =>
{
    var result = await userService.RegisterAsync(dto);
    return result.Match<IResult>(
        user => Results.Ok(user),
        error => Results.BadRequest("No jeb³o"),
        _ => Results.NotFound()
        );
})
.WithName("ApiRegister")
.WithOpenApi();

app.MapPost("/api/login", (RegisterLoginUserDto dto, IUserService userService) =>
{
    var result = userService.Login(dto);
    return result.Match<IResult>(
        user => Results.Ok(user),
        error => Results.BadRequest("zue chas³o"),
        _ => Results.NotFound()
        );
})
.WithName("ApiLogin")
.WithOpenApi();

app.MapPost("/api/job", async (CreateJobDto dto, IJobService jobService) =>
{
    var result = await jobService.AddJobAsync(dto);
    return result;
})
.WithName("ApiJob")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
