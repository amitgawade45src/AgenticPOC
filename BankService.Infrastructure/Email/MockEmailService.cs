using BankService.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.Email;

/// <summary>
/// Mock implementation of <see cref="IEmailService"/> that simulates sending an
/// email by logging it. In a real system this would call an SMTP server or an
/// email provider's API.
/// </summary>
public sealed class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;

    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending (mock) email to {To}...", to);

        // Simulate the latency of calling an external email provider.
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation("Mock email sent.\n  To: {To}\n  Subject: {Subject}\n  Body: {Body}", to, subject, body);
    }
}
