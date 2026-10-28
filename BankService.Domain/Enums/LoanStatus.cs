namespace BankService.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a loan.
/// </summary>
public enum LoanStatus
{
    Applied = 0,
    Approved = 1,
    Rejected = 2,
    Disbursed = 3,
    Closed = 4
}
