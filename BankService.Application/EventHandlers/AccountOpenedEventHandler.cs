using BankService.Application.Interfaces;
using BankService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankService.Application.EventHandlers;

/// <summary>
/// Handles the <see cref="AccountOpenedEvent"/> domain event by sending the
/// customer a welcome email. Reacting to the domain event keeps this side effect
/// decoupled from the account-opening command handler.
/// </summary>
public sealed class AccountOpenedEventHandler : INotificationHandler<AccountOpenedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountOpenedEventHandler> _logger;

    public AccountOpenedEventHandler(IEmailService emailService, ILogger<AccountOpenedEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(AccountOpenedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling AccountOpenedEvent for customer {CustomerId}, account {AccountNumber}.",
            notification.CustomerId, notification.AccountNumber);

        // In a real system the customer's email would be looked up; mocked here.
        var to = $"customer-{notification.CustomerId}@example.com";
        var subject = "Welcome to the bank \u2013 your account is open";
        var body =
            $"Dear customer,\n\nYour new account {notification.AccountNumber} has been " +
            $"opened successfully on {notification.OccurredOnUtc:u}.\n\nThank you for banking with us.";

        await _emailService.SendAsync(to, subject, body, cancellationToken);
    }
}
