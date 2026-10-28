using BankService.Domain.Common;
using BankService.Domain.Enums;
using BankService.Domain.Events;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Entities;

/// <summary>
/// Aggregate root for a short-term unsecured loan.
/// </summary>
public sealed class InstantLoan : AggregateRoot
{
    public const decimal MaximumLoanAmount = 10000m;

    public CustomerId CustomerId { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public decimal ApprovedAmount { get; private set; }
    public LoanStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    private InstantLoan()
    {
    }

    private InstantLoan(CustomerId customerId, decimal requestedAmount, DateTime createdOnUtc)
        : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        RequestedAmount = requestedAmount;
        Status = LoanStatus.Applied;
        CreatedOnUtc = createdOnUtc;
    }

    public static InstantLoan Apply(CustomerId customerId, decimal requestedAmount, DateTime createdOnUtc)
    {
        if (requestedAmount <= 0 || requestedAmount > MaximumLoanAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(requestedAmount), $"Requested amount must be between 0 and {MaximumLoanAmount}.");
        }

        var loan = new InstantLoan(customerId, requestedAmount, createdOnUtc);
        loan.RaiseDomainEvent(new InstantLoanAppliedEvent(loan.Id, customerId, requestedAmount, createdOnUtc));
        return loan;
    }

    public void Approve(decimal approvedAmount, DateTime approvedOnUtc)
    {
        if (approvedAmount <= 0 || approvedAmount > RequestedAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(approvedAmount), "Approved amount must be greater than zero and cannot exceed the requested amount.");
        }

        ApprovedAmount = approvedAmount;
        Status = LoanStatus.Approved;
        RaiseDomainEvent(new InstantLoanApprovedEvent(Id, CustomerId, approvedAmount, approvedOnUtc));
    }

    public void Reject(DateTime rejectedOnUtc)
    {
        Status = LoanStatus.Rejected;
        RaiseDomainEvent(new InstantLoanRejectedEvent(Id, CustomerId, rejectedOnUtc));
    }

    public void Disburse(DateTime disbursedOnUtc)
    {
        Status = LoanStatus.Disbursed;
        RaiseDomainEvent(new InstantLoanDisbursedEvent(Id, CustomerId, ApprovedAmount, disbursedOnUtc));
    }

    public void Repay(decimal amount, DateTime repaidOnUtc)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Repayment amount must be greater than zero.");
        }

        RaiseDomainEvent(new InstantLoanRepaymentReceivedEvent(Id, CustomerId, amount, repaidOnUtc));

        if (amount >= ApprovedAmount && ApprovedAmount > 0)
        {
            Status = LoanStatus.Closed;
        }
    }

    public static InstantLoan Restore(
        Guid id,
        CustomerId customerId,
        decimal requestedAmount,
        decimal approvedAmount,
        LoanStatus status,
        DateTime createdOnUtc)
    {
        return new InstantLoan
        {
            Id = id,
            CustomerId = customerId,
            RequestedAmount = requestedAmount,
            ApprovedAmount = approvedAmount,
            Status = status,
            CreatedOnUtc = createdOnUtc
        };
    }
}
