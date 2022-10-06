using ArtGallery.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Application;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using System.Text.Json.Serialization;
using ArtGallery.Api.Filters;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add Logger Config
var levelSwitch = new LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("ApplicationName", "ArtGallery")
    .Enrich.WithProperty("ApplicationVersion", "0.1")
    .MinimumLevel.ControlledBy(levelSwitch)
    .WriteTo.Console()
    .WriteTo.File($"{Directory.GetCurrentDirectory()}/logs/ServiceLog.txt", rollingInterval: RollingInterval.Hour)
    //.WriteTo.Seq(builder.Configuration.GetValue<string>("Seq:serverUrl"), controlLevelSwitch: levelSwitch)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddApplicationLayer();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext"), opt => opt.EnableRetryOnFailure()));

builder.Services.AddScoped<IApplicationContext, ApplicationContext>();

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
