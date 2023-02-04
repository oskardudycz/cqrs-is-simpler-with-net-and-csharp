using Core.WebApi.Middlewares.ExceptionHandling;
using Warehouse;
using Warehouse.Api.Middlewares.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlingMiddleware()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
    })
    //.ConfigureWarehouse()
    .UseSwagger()
    .UseSwaggerUI();

app.Run();

public partial class Program
{
}
