using BankService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.CreditScore;

/// <summary>
/// Mock implementation of <see cref="ICreditScoreService"/> that simulates calling
/// an external credit bureau / earning-score API. The score is derived
/// deterministically from the customer id so the same customer always gets the
/// same result, which is convenient for a demo.
/// </summary>
public sealed class MockCreditScoreService : ICreditScoreService
{
    private readonly ILogger<MockCreditScoreService> _logger;

    public MockCreditScoreService(ILogger<MockCreditScoreService> logger)
    {
        _logger = logger;
    }

    public async Task<CreditScoreResult> GetEarningScoreAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calling external (mock) credit-score API for {CustomerId}...", customerId);

        // Simulate network latency of an out-of-process API call.
        await Task.Delay(150, cancellationToken);

        // Deterministically map the customer id to a score in the range 300-850.
        var score = 300 + Math.Abs(customerId.GetHashCode() % 551);

        _logger.LogInformation("Mock credit-score API returned {Score} for {CustomerId}.", score, customerId);

        return new CreditScoreResult(score);
    }
}
