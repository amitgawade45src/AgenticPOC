using BankService.Application.DTOs;
using BankService.Application.UseCases.Commands.Lending;
using BankService.Application.UseCases.Queries.Lending;
using BankService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Controllers;

/// <summary>
/// Lending bounded context – Home Loan: property financing linked to a customer's bank account.
/// </summary>
[ApiController]
[Route("api/home-loans")]
[Tags("Home Loans")]
[Produces("application/json")]
public sealed class HomeLoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomeLoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Applies for a home loan.</summary>
    /// <remarks>Loan amount and term must be greater than zero. Raises <c>HomeLoanApplied</c>.</remarks>
    [HttpPost]
    [EndpointSummary("Apply for home loan")]
    [ProducesResponseType(typeof(HomeLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Apply([FromBody] ApplyForHomeLoanRequest request, CancellationToken cancellationToken)
    {
        var loan = await _mediator.Send(new ApplyForHomeLoanCommand(
            request.CustomerId,
            request.AccountId,
            request.PropertyAddress,
            request.LoanAmount,
            request.InterestRate,
            request.LoanTermMonths), cancellationToken);

        return Ok(loan);
    }

    /// <summary>Approves a home loan application.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/approve")]
    [EndpointSummary("Approve home loan")]
    [ProducesResponseType(typeof(HomeLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(Guid loanId, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new ApproveHomeLoanCommand(loanId), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Rejects a home loan application.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/reject")]
    [EndpointSummary("Reject home loan")]
    [ProducesResponseType(typeof(HomeLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid loanId, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new RejectHomeLoanCommand(loanId), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Disburses an approved home loan.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/disburse")]
    [EndpointSummary("Disburse home loan")]
    [ProducesResponseType(typeof(HomeLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Disburse(Guid loanId, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new DisburseHomeLoanCommand(loanId), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Pays an EMI instalment, reducing the outstanding amount.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpPost("{loanId:guid}/pay-emi")]
    [EndpointSummary("Pay EMI")]
    [ProducesResponseType(typeof(HomeLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PayEmi(Guid loanId, [FromBody] PayEmiRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _mediator.Send(new PayEmiCommand(loanId, request.Amount), cancellationToken);
            return Ok(loan);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Gets home loan details.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpGet("{loanId:guid}")]
    [EndpointSummary("Get home loan details")]
    [ProducesResponseType(typeof(HomeLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid loanId, CancellationToken cancellationToken)
    {
        var loan = await _mediator.Send(new GetHomeLoanDetailsQuery(loanId), cancellationToken);
        return loan is null ? NotFound() : Ok(loan);
    }

    /// <summary>Gets the outstanding amount for a home loan.</summary>
    /// <param name="loanId">The loan identifier.</param>
    [HttpGet("{loanId:guid}/outstanding-amount")]
    [EndpointSummary("Get outstanding amount")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOutstandingAmount(Guid loanId, CancellationToken cancellationToken)
    {
        var outstandingAmount = await _mediator.Send(new GetOutstandingAmountQuery(loanId), cancellationToken);
        return outstandingAmount is null ? NotFound() : Ok(outstandingAmount.Value);
    }

    /// <summary>Lists all home loans for a customer.</summary>
    /// <param name="customerId">The customer identifier.</param>
    [HttpGet("customer/{customerId:guid}")]
    [EndpointSummary("Get customer home loans")]
    [ProducesResponseType(typeof(IReadOnlyList<HomeLoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        var loans = await _mediator.Send(new GetCustomerHomeLoansQuery(customerId), cancellationToken);
        return Ok(loans);
    }
}
