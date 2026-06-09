using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

/// <summary>
/// Custom mapper abstraction for translating between the domain
/// <see cref="BankAccount"/> aggregate and the infrastructure
/// <see cref="BankAccountRecord"/> persistence model. The implementation is backed
/// by AutoMapper, but the rest of the infrastructure depends only on this
/// intention-revealing contract.
/// </summary>
public interface IBankAccountMapper
{
    BankAccountRecord ToRecord(BankAccount account);

    BankAccount ToDomain(BankAccountRecord record);
}
