namespace BankService.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a <see cref="Entities.BankAccount"/>.
/// </summary>
public enum AccountStatus
{
    /// <summary>The application was received but not yet decided.</summary>
    Pending = 0,

    /// <summary>The customer passed eligibility and the account is active.</summary>
    Open = 1,

    /// <summary>The customer failed eligibility; no account was opened.</summary>
    Rejected = 2,

    /// <summary>A previously opened account that has been closed.</summary>
    Closed = 3
}
