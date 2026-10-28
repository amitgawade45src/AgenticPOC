using AutoMapper;
using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

public sealed class InstantLoanMapper : IInstantLoanMapper
{
    private readonly IMapper _mapper;

    public InstantLoanMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile<PersistenceMappingProfile>());

        _mapper = configuration.CreateMapper();
    }

    public InstantLoanRecord ToRecord(InstantLoan loan) => _mapper.Map<InstantLoanRecord>(loan);

    public InstantLoan ToDomain(InstantLoanRecord record) => _mapper.Map<InstantLoan>(record);
}
