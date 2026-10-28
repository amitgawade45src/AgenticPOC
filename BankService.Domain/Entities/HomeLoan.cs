using BankService.Domain.Common;
using BankService.Domain.Enums;
using BankService.Domain.Events;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Entities;

/// <summary>
/// Aggregate root for a property financing product.
/// </summary>
public sealed class HomeLoan : AggregateRoot
{
    public CustomerId CustomerId { get; private set; }
    public Guid AccountId { get; private set; }
    public string PropertyAddress { get; private set; } = string.Empty;
    public decimal LoanAmount { get; private set; }
    public decimal OutstandingAmount { get; private set; }
    public decimal InterestRate { get; private set; }
    public int LoanTermMonths { get; private set; }
    public LoanStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    private HomeLoan()
    {
    }

    private HomeLoan(
        CustomerId customerId,
        Guid accountId,
        string propertyAddress,
        decimal loanAmount,
        decimal interestRate,
        int loanTermMonths,
        DateTime createdOnUtc)
        : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        AccountId = accountId;
        PropertyAddress = propertyAddress;
        LoanAmount = loanAmount;
        OutstandingAmount = loanAmount;
        InterestRate = interestRate;
        LoanTermMonths = loanTermMonths;
        Status = LoanStatus.Applied;
        CreatedOnUtc = createdOnUtc;
    }

    public static HomeLoan Apply(
        CustomerId customerId,
        Guid accountId,
        string propertyAddress,
        decimal loanAmount,
        decimal interestRate,
        int loanTermMonths,
        DateTime createdOnUtc)
    {
        if (loanAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loanAmount), "Loan amount must be greater than zero.");
        }

        if (loanTermMonths <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(loanTermMonths), "Loan term must be greater than zero.");
        }

        var loan = new HomeLoan(customerId, accountId, propertyAddress, loanAmount, interestRate, loanTermMonths, createdOnUtc);
        loan.RaiseDomainEvent(new HomeLoanAppliedEvent(loan.Id, customerId, accountId, loanAmount, createdOnUtc));
        return loan;
    }

    public void Approve(DateTime approvedOnUtc)
    {
        Status = LoanStatus.Approved;
        RaiseDomainEvent(new HomeLoanApprovedEvent(Id, CustomerId, AccountId, LoanAmount, approvedOnUtc));
    }

    public void Reject(DateTime rejectedOnUtc)
    {
        Status = LoanStatus.Rejected;
        RaiseDomainEvent(new HomeLoanRejectedEvent(Id, CustomerId, AccountId, rejectedOnUtc));
    }

    public void Disburse(DateTime disbursedOnUtc)
    {
        Status = LoanStatus.Disbursed;
        RaiseDomainEvent(new HomeLoanDisbursedEvent(Id, CustomerId, AccountId, LoanAmount, disbursedOnUtc));
    }

    public void PayEmi(decimal amount, DateTime paidOnUtc)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "EMI amount must be greater than zero.");
        }

        OutstandingAmount = Math.Max(0m, OutstandingAmount - amount);
        RaiseDomainEvent(new EmiPaidEvent(Id, CustomerId, AccountId, amount, OutstandingAmount, paidOnUtc));

        if (OutstandingAmount == 0m)
        {
            Status = LoanStatus.Closed;
        }
    }

    public static HomeLoan Restore(
        Guid id,
        CustomerId customerId,
        Guid accountId,
        string propertyAddress,
        decimal loanAmount,
        decimal outstandingAmount,
        decimal interestRate,
        int loanTermMonths,
        LoanStatus status,
        DateTime createdOnUtc)
    {
        return new HomeLoan
        {
            Id = id,
            CustomerId = customerId,
            AccountId = accountId,
            PropertyAddress = propertyAddress,
            LoanAmount = loanAmount,
            OutstandingAmount = outstandingAmount,
            InterestRate = interestRate,
            LoanTermMonths = loanTermMonths,
            Status = status,
            CreatedOnUtc = createdOnUtc
        };
    }
}
