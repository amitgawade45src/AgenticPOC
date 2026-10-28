using BankService.Application.DTOs;
using BankService.Application.UseCases.Commands.Lending;
using BankService.Application.UseCases.Queries.Lending;
using BankService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Controllers;

/// <summary>
/// Lending bounded context – Instant Loan: short-term unsecured loan up to $10,000.
/// A customer can apply without owning a bank account.
/// </summary>
[ApiController]
[Route("api/instant-loans")]
[Tags("Instant Loans")]
[Produces("application/json")]
public sealed class InstantLoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstantLoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Applies for an instant loan.</summary>
    /// <remarks>Requested amount must be between 0 and 10,000. Raises <c>InstantLoanApplied</c>.</remarks>
    [HttpPost]
    [EndpointSummary("Apply for instant loan")]
    [ProducesResponseType(typeof(InstantLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Apply([FromBody] ApplyForInstantLoanRequest request, CancellationToken cancellationToken)
    {
        var loan = await _mediator.Send(new ApplyForInstantLoanCommand(request.CustomerId, request.RequestedAmount), cancellationToken);
        return Ok(loan);
    }

    /// <summary>Approves an instant loan for the given amount.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/approve")]
    [EndpointSummary("Approve instant loan")]
    [ProducesResponseType(typeof(InstantLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(Guid loanId, [FromBody] ApproveInstantLoanRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new ApproveInstantLoanCommand(loanId, request.ApprovedAmount), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Rejects an instant loan application.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/reject")]
    [EndpointSummary("Reject instant loan")]
    [ProducesResponseType(typeof(InstantLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid loanId, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new RejectInstantLoanCommand(loanId), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Disburses an approved instant loan.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/disburse")]
    [EndpointSummary("Disburse instant loan")]
    [ProducesResponseType(typeof(InstantLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Disburse(Guid loanId, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new DisburseInstantLoanCommand(loanId), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Records a repayment against an instant loan.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/repay")]
    [EndpointSummary("Repay instant loan")]
    [ProducesResponseType(typeof(InstantLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Repay(Guid loanId, [FromBody] RepayInstantLoanRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new RepayInstantLoanCommand(loanId, request.Amount), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Gets instant loan details.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpGet("{loanId:guid}")]
    [EndpointSummary("Get instant loan details")]
    [ProducesResponseType(typeof(InstantLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid loanId, CancellationToken cancellationToken)
    {
        var loan = await _mediator.Send(new GetInstantLoanDetailsQuery(loanId), cancellationToken);
        return loan is null ? NotFound() : Ok(loan);
    }

    /// <summary>Lists all instant loans for a customer.</summary>
    /// <param name="customerId">The customer identifier.</param>
    [HttpGet("customer/{customerId:guid}")]
    [EndpointSummary("Get customer instant loans")]
    [ProducesResponseType(typeof(IReadOnlyList<InstantLoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        var loans = await _mediator.Send(new GetCustomerInstantLoansQuery(customerId), cancellationToken);
        return Ok(loans);
    }
}
