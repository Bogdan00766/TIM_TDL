using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TIM_TDL.Application.Dtos.Job;
using TIM_TDL.Application.Dtos.User;
using TIM_TDL.Application.IServices;
using TIM_TDL.Application.Services;
using TIM_TDL.Application.Utilities;
using TIM_TDL.Domain.IRepositories;
using TIM_TDL.Infrastructure;
using TIM_TDL.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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


app.MapPost("/api/register", async (RegisterLoginUserDto dto, IUserService userService) =>
{
    var result = await userService.RegisterAsync(dto);
    return result.Match<IResult>(
        user => Results.Ok(user),
        error => Results.BadRequest("No jeb�o"),
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
        error => Results.BadRequest("zue chas�o"),
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
