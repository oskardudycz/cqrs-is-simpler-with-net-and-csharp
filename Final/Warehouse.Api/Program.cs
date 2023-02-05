using Warehouse;
using Warehouse.Api.Middlewares.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting()
    .AddWarehouseServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlingMiddleware()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.UseWarehouseEndpoints();
    })
    .ConfigureWarehouse()
    .UseSwagger()
    .UseSwaggerUI();

app.Run();

public partial class Program
{
}
