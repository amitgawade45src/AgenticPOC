using BankService.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankService.Application;

/// <summary>
/// Registers all application-layer services: MediatR handlers, the validation
/// pipeline behavior, FluentValidation validators, and AutoMapper profiles.
/// </summary>
public static class DependencyInjection
{
    /// <summary>Configuration key holding the AutoMapper (Lucky Penny Software) license key.</summary>
    public const string AutoMapperLicenseKeyConfigurationKey = "AutoMapper:LicenseKey";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var licenseKey = GetAutoMapperLicenseKey(configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(cfg => cfg.LicenseKey = licenseKey, assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    /// <summary>Reads the AutoMapper license key, returning <c>null</c> when it is missing or blank.</summary>
    public static string? GetAutoMapperLicenseKey(IConfiguration configuration)
    {
        var key = configuration[AutoMapperLicenseKeyConfigurationKey];
        return string.IsNullOrWhiteSpace(key) ? null : key;
    }
}
