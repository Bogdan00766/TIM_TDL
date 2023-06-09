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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TIM_TDL.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

//builder.Services.AddDbContext<TDLDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:TIMConnection"]));
builder.Services.AddDbContext<TDLDbContext>(options => options.UseSqlite("Data Source=localDB.db"));

builder.Configuration.AddEnvironmentVariables("ENV_");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);
var secretKey = builder.Configuration["Keys:JWT"];

//To Add-Migration comment line below
if (secretKey == null ||secretKey.Length == 0) throw new Exception("No JWT key");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            RequireAudience = true,
            RequireSignedTokens = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Keys:Issuer"],
            //IgnoreTrailingSlashWhenValidatingAudience = true,
            ValidAudience = builder.Configuration["Keys:Audience"],
            ValidateAudience = true,
        };
    }
    );
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));


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
app.UseRouting();
app.UseAuthorization();

app.MapPost("/api/register", async (RegisterUserDto dto, IUserService userService) =>
{
    var result = await userService.RegisterAsync(dto);
    return result.Match<IResult>(
        user => Results.Ok(user),
        error => Results.BadRequest("Error during register process"),
        _ => Results.NotFound()
        );
})
.WithName("ApiRegister")
.WithOpenApi();

app.MapPost("/api/login", (LoginUserDto dto, IUserService userService) =>
{
    var result = userService.Login(dto);
    return result.Match<IResult>(
        user => Results.Ok(user),
        error => Results.Unauthorized(),
        _ => Results.NotFound()
        );
})
.WithName("ApiLogin")
.WithOpenApi();

//CRUD JOB
//CREATE
app.MapPost("/api/createJob", async (CreateJobDto dto, HttpContext context, IJobService jobService) =>
{

    var result = await jobService.CreateJobAsync(dto, context);
    return result;
})
.WithName("ApiCreateJob")
.WithOpenApi()
.RequireAuthorization();

//READ
app.MapGet("/api/readJob", (HttpContext context, IJobService jobService) =>
{
    var result = jobService.ReadJob(context);
    return result;
})
.WithName("ApiReadJob")
.WithOpenApi()
.RequireAuthorization();
//UPDATE

app.MapPut("/api/updateJob", async (ReadUpdateJobDto dto, HttpContext context, IJobService jobService) =>
{
    var result = await jobService.UpdateJobAsync(dto, context);
    return result;
})
.WithName("ApiUpdateJob")
.WithOpenApi()
.RequireAuthorization();


app.MapPut("/api/changePassword", async (ChangePasswordUser dto, HttpContext context, IUserService userService) =>
{
    var result = await userService.ChangePasswordAsync(dto, context);
    return result.Match<IResult>(
        user => Results.Ok(user),
        error => Results.StatusCode(500)
        );
})
.WithName("ApiChangePassword")
.WithOpenApi()
.RequireAuthorization();
app.Run();


internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
