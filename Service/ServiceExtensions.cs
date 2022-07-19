using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace Service;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDataCalculator, DataCalculator>();
        return services;
    }
}