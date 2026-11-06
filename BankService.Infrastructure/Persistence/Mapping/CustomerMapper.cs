using AutoMapper;
using BankService.Domain.Entities;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

public sealed class CustomerMapper : ICustomerMapper
{
    private readonly IMapper _mapper;

    public CustomerMapper(PersistenceMapper persistenceMapper)
    {
        _mapper = persistenceMapper.Mapper;
    }

    public CustomerRecord ToRecord(Customer customer) => _mapper.Map<CustomerRecord>(customer);

    public Customer ToDomain(CustomerRecord record) => _mapper.Map<Customer>(record);
}
