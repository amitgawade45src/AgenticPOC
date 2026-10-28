using AutoMapper;
using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

public sealed class HomeLoanMapper : IHomeLoanMapper
{
    private readonly IMapper _mapper;

    public HomeLoanMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile<PersistenceMappingProfile>());

        _mapper = configuration.CreateMapper();
    }

    public HomeLoanRecord ToRecord(HomeLoan loan) => _mapper.Map<HomeLoanRecord>(loan);

    public HomeLoan ToDomain(HomeLoanRecord record) => _mapper.Map<HomeLoan>(record);
}
