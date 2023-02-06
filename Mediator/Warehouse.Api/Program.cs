using Warehouse;
using Warehouse.Api.Middlewares.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWarehouseServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var app = builder.Build();

app.UseExceptionHandlingMiddleware()
    .UseRouting()
    .UseEndpoints(endpoints =>
        endpoints.MapControllers()
    )
    .ConfigureWarehouse()
    .UseSwagger()
    .UseSwaggerUI();

app.Run();

namespace Warehouse.Api
{
    public partial class Program
    {
    }
}
