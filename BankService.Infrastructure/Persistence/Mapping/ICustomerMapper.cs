using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

public interface ICustomerMapper
{
    CustomerRecord ToRecord(Customer customer);

    Customer ToDomain(CustomerRecord record);
}
