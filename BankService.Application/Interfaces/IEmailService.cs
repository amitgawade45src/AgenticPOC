namespace BankService.Application.Interfaces;

/// <summary>
/// Port for sending emails. The implementation (a mock email sender) lives in the
/// infrastructure layer.
/// </summary>
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
