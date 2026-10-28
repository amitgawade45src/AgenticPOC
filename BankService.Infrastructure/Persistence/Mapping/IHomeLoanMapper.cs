using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

public interface IHomeLoanMapper
{
    HomeLoanRecord ToRecord(HomeLoan loan);

    HomeLoan ToDomain(HomeLoanRecord record);
}
