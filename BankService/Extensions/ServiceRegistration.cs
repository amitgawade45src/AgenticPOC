using BankService.Application;
using BankService.Infrastructure;

namespace BankService.Extensions;

/// <summary>
/// Presentation-layer composition root. This is where the outer layer wires up the
/// inner layers (Application + Infrastructure) without the inner layers knowing
/// about the web host.
/// </summary>
public static class ServiceRegistration
{
    public static IServiceCollection AddBankServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration); 
        return services;
    }
}
