using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using BankService.Infrastructure.Persistence.Mapping;

namespace BankService.Infrastructure.Persistence;

public sealed class HomeLoanRepository : IHomeLoanRepository
{
    private readonly InMemoryBankStore _store;
    private readonly InMemoryChangeTracker _changeTracker;
    private readonly IHomeLoanMapper _mapper;

    public HomeLoanRepository(
        InMemoryBankStore store,
        InMemoryChangeTracker changeTracker,
        IHomeLoanMapper mapper)
    {
        _store = store;
        _changeTracker = changeTracker;
        _mapper = mapper;
    }

    public Task AddAsync(HomeLoan aggregate, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingHomeLoans.Add(_mapper.ToRecord(aggregate));
        return Task.CompletedTask;
    }

    public Task UpdateAsync(HomeLoan aggregate, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingHomeLoans.RemoveAll(l => l.Id == aggregate.Id);
        _changeTracker.PendingHomeLoans.Add(_mapper.ToRecord(aggregate));
        return Task.CompletedTask;
    }

    public Task<HomeLoan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingHomeLoans.FirstOrDefault(l => l.Id == id)
            ?? (_store.HomeLoans.TryGetValue(id, out var stored) ? stored : null);

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }

    public Task<IReadOnlyList<HomeLoan>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    {
        var records = _store.HomeLoans.Values
            .Concat(_changeTracker.PendingHomeLoans)
            .GroupBy(l => l.Id)
            .Select(g => g.Last())
            .Where(l => l.CustomerId == customerId.Value)
            .Select(_mapper.ToDomain)
            .ToList();

        return Task.FromResult<IReadOnlyList<HomeLoan>>(records);
    }
}
