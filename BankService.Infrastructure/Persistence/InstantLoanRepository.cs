using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using BankService.Infrastructure.Persistence.Mapping;

namespace BankService.Infrastructure.Persistence;

public sealed class InstantLoanRepository : IInstantLoanRepository
{
    private readonly InMemoryBankStore _store;
    private readonly InMemoryChangeTracker _changeTracker;
    private readonly IInstantLoanMapper _mapper;

    public InstantLoanRepository(
        InMemoryBankStore store,
        InMemoryChangeTracker changeTracker,
        IInstantLoanMapper mapper)
    {
        _store = store;
        _changeTracker = changeTracker;
        _mapper = mapper;
    }

    public Task AddAsync(InstantLoan aggregate, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingInstantLoans.Add(_mapper.ToRecord(aggregate));
        return Task.CompletedTask;
    }

    public Task UpdateAsync(InstantLoan aggregate, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingInstantLoans.RemoveAll(l => l.Id == aggregate.Id);
        _changeTracker.PendingInstantLoans.Add(_mapper.ToRecord(aggregate));
        return Task.CompletedTask;
    }

    public Task<InstantLoan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingInstantLoans.FirstOrDefault(l => l.Id == id)
            ?? (_store.InstantLoans.TryGetValue(id, out var stored) ? stored : null);

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }

    public Task<IReadOnlyList<InstantLoan>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    {
        var records = _store.InstantLoans.Values
            .Concat(_changeTracker.PendingInstantLoans)
            .GroupBy(l => l.Id)
            .Select(g => g.Last())
            .Where(l => l.CustomerId == customerId.Value)
            .Select(_mapper.ToDomain)
            .ToList();

        return Task.FromResult<IReadOnlyList<InstantLoan>>(records);
    }
}
