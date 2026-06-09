using AutoMapper;
using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

/// <summary>
/// Custom mapper that wraps AutoMapper to convert between the domain
/// <see cref="BankAccount"/> aggregate and the <see cref="BankAccountRecord"/>
/// persistence model. It owns a self-contained AutoMapper configuration so the
/// infrastructure mapping is independent of the application layer's AutoMapper setup.
/// </summary>
public sealed class BankAccountMapper : IBankAccountMapper
{
    private readonly IMapper _mapper;

    public BankAccountMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile<PersistenceMappingProfile>());

        _mapper = configuration.CreateMapper();
    }

    public BankAccountRecord ToRecord(BankAccount account) =>
        _mapper.Map<BankAccountRecord>(account);

    public BankAccount ToDomain(BankAccountRecord record) =>
        _mapper.Map<BankAccount>(record);
}
