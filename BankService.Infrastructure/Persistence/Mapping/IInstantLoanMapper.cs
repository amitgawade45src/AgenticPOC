using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

public interface IInstantLoanMapper
{
    InstantLoanRecord ToRecord(InstantLoan loan);

    InstantLoan ToDomain(InstantLoanRecord record);
}
