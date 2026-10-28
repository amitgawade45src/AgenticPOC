namespace BankService.Models;

/// <summary>
/// Request body for approving an instant loan.
/// </summary>
public sealed class ApproveInstantLoanRequest
{
    public decimal ApprovedAmount { get; set; }
}
