using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TimeSeriesAnalyticsService.Application.Services;
using TimeSeriesAnalyticsService.Application.UseCases;
using TimeSeriesAnalyticsService.Application.Validation;
using TimeSeriesAnalyticsService.Domain.Services;
using TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TimeSeriesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<TimeSeriesValidator>();

builder.Services.AddScoped<IResultsCalculator, ResultsCalculator>();
builder.Services.AddScoped<ITimeSeriesStorage, TimeSeriesStorageEf>();

builder.Services.AddScoped<TimeSeriesCsvParser>();
builder.Services.AddScoped<ImportTimeSeriesCsv>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
