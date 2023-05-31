using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerHealthCheckAPI.Data;
using CustomerHealthCheckAPI.Database;
using CustomerHealthCheckAPI.Services;
using CustomerHealthCheckAPI.Health;
using HealthChecks.UI.Client;
using System.Security.Policy;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var storage = builder.Configuration.GetSection("StorageAccountConnectionString").Value;
builder.Services.AddSingleton(x => new BlobServiceClient(storage));
builder.Services.AddDbContext<CustomerHealthCheckAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerHealthCheckAPIContext") ?? throw new InvalidOperationException("Connection string 'CustomerHealthCheckAPIContext' not found.")));
builder.Services.AddSingleton<DatabaseHealthCheck>(new DatabaseHealthCheck(builder.Configuration.GetSection("CustomerHealthCheckAPIContext").Value));
builder.Services.AddScoped<IGitHubService, GitHubService>();
// Add services to the container.
builder.Services.AddHealthChecks()
    // .AddCheck<DatabaseHealthCheck>("Database").
    .AddSqlServer(builder.Configuration.GetConnectionString("CustomerHealthCheckAPIContext"))
   .AddAzureBlobStorage(builder.Configuration.GetSection("StorageAccountConnectionString").Value)
   .AddAzureServiceBusQueue(builder.Configuration.GetSection("OnBoardingServiceBus:ConnectionString").Value, builder.Configuration.GetSection("OnBoardingServiceBus:QueueName").Value).
    AddCheck<GitHubHealthChecker>("GitHub");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("_health",new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseAuthorization();

app.MapControllers();

app.Run();
