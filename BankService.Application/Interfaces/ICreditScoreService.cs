namespace BankService.Application.Interfaces;

/// <summary>
/// Result returned by the external credit-score provider.
/// </summary>
public sealed record CreditScoreResult(int EarningScore);

/// <summary>
/// Port for obtaining a customer's earning/credit score. The implementation
/// (a mock external API) lives in the infrastructure layer.
/// </summary>
public interface ICreditScoreService
{
    Task<CreditScoreResult> GetEarningScoreAsync(Guid customerId, CancellationToken cancellationToken = default);
}
