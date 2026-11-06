using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace BankService.Infrastructure.Persistence.Mapping;

/// <summary>
/// Builds the single AutoMapper configuration used by the persistence mappers
/// (domain aggregate &lt;-&gt; persistence record). Centralising it here means the
/// license key is applied once and the (expensive) configuration is built once.
/// </summary>
public static class PersistenceMapperFactory
{
    public static PersistenceMapper Create(string? licenseKey)
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.LicenseKey = licenseKey;
            cfg.AddProfile<PersistenceMappingProfile>();
        }, NullLoggerFactory.Instance);

        return new PersistenceMapper(configuration.CreateMapper());
    }
}

/// <summary>
/// Strongly-typed wrapper so the persistence <see cref="IMapper"/> can be resolved
/// without colliding with the application layer's <see cref="IMapper"/> registration.
/// </summary>
public sealed class PersistenceMapper
{
    public PersistenceMapper(IMapper mapper)
    {
        Mapper = mapper;
    }

    public IMapper Mapper { get; }
}
