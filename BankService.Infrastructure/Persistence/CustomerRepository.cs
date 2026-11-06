using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using BankService.Infrastructure.Persistence.Mapping;

namespace BankService.Infrastructure.Persistence;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly InMemoryBankStore _store;
    private readonly InMemoryChangeTracker _changeTracker;
    private readonly ICustomerMapper _mapper;

    public CustomerRepository(
        InMemoryBankStore store,
        InMemoryChangeTracker changeTracker,
        ICustomerMapper mapper)
    {
        _store = store;
        _changeTracker = changeTracker;
        _mapper = mapper;
    }

    public Task AddAsync(Customer aggregate, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingCustomers.Add(_mapper.ToRecord(aggregate));
        _changeTracker.Track(aggregate);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Customer aggregate, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingCustomers.RemoveAll(c => c.Id == aggregate.Id);
        _changeTracker.PendingCustomers.Add(_mapper.ToRecord(aggregate));
        _changeTracker.Track(aggregate);
        return Task.CompletedTask;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingCustomers.FirstOrDefault(c => c.Id == id)
            ?? (_store.Customers.TryGetValue(id, out var stored) ? stored : null);

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }

    public Task<Customer?> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingCustomers.FirstOrDefault(c => c.CustomerId == customerId.Value)
            ?? _store.Customers.Values.FirstOrDefault(c => c.CustomerId == customerId.Value);

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }

    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingCustomers.FirstOrDefault(c => string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase))
            ?? _store.Customers.Values.FirstOrDefault(c => string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }

    public Task<IReadOnlyList<Customer>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var allRecords = _store.Customers.Values
            .Concat(_changeTracker.PendingCustomers)
            .GroupBy(c => c.Id)
            .Select(g => g.Last())
            .Where(c => string.IsNullOrWhiteSpace(searchTerm)
                || c.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || c.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                || c.MobileNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Select(_mapper.ToDomain)
            .ToList();

        return Task.FromResult<IReadOnlyList<Customer>>(allRecords);
    }
}
