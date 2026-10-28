using BankService.Application.DTOs;
using BankService.Application.UseCases.Commands.OpenAccount;
using BankService.Application.UseCases.Commands.Banking;
using BankService.Application.UseCases.Queries.Banking;
using BankService.Application.UseCases.Queries.GetAccount;
using BankService.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankService.Controllers;

/// <summary>
/// Banking bounded context: open a bank account, manage its onboarding documents and close it.
/// This is the presentation layer: it translates HTTP into application commands/queries
/// via MediatR and translates the results back into HTTP responses. It contains no business logic.
/// </summary>
[ApiController]
[Route("api/accounts")]
[Tags("Bank Accounts")]
[Produces("application/json")]
public sealed class AccountOpeningController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountOpeningController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the "Apply to open account" action. Accepts the customer details and
    /// an uploaded document (multipart/form-data).
    /// </summary>
    /// <remarks>
    /// The customer's earning score is retrieved from the credit-score provider; the account is
    /// opened only when the score meets the eligibility minimum (650). On success an
    /// <c>AccountOpened</c> event is published and a welcome email is sent.
    /// Eligible -> 200; ineligible (business-rule rejection) -> 422.
    /// </remarks>
    [HttpPost("open")]
    [EndpointSummary("Open bank account")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(OpenAccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OpenAccountResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Open([FromForm] OpenAccountRequest request, CancellationToken cancellationToken)
    {
        await using var documentStream = request.Document.OpenReadStream();

        var command = new OpenAccountCommand(Guid.NewGuid(), request.FullName, documentStream, request.Document.FileName, request.Document.ContentType);

        var result = await _mediator.Send(command, cancellationToken);

        var response = new OpenAccountResponse
        {
            IsApproved = result.IsApproved,
            Message = result.Message,
            AccountId = result.Account?.Id,
            AccountNumber = result.Account?.AccountNumber,
            Status = result.Account?.Status,
            Documents = result.Account?.Documents
                .Select(d => new AccountDocument
                {
                    Id = d.Id,
                    Uri = d.Uri,
                    FileName = d.FileName,
                    VerificationStatus = d.VerificationStatus,
                    UploadedOnUtc = d.UploadedOnUtc
                })
                .ToList() ?? []
        };

        // Eligible -> 200; ineligible (business-rule rejection) -> 422.
        return result.IsApproved ? Ok(response) : UnprocessableEntity(response);
    }

    /// <summary>
    /// Retrieves a previously opened account by id.
    /// </summary>
    /// <param name="accountId">The account identifier.</param>
    [HttpGet("{accountId:guid}")]
    [EndpointSummary("Get account details")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await _mediator.Send(new GetAccountQuery(accountId), cancellationToken);
        return account is null ? NotFound() : Ok(account);
    }

    /// <summary>Uploads and attaches an additional onboarding document to an account.</summary>
    /// <param name="accountId">The account identifier.</param>
    [HttpPost("{accountId:guid}/documents")]
    [EndpointSummary("Add account document")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddDocument(Guid accountId, [FromForm] AddAccountDocumentRequest request, CancellationToken cancellationToken)
    {
        await using var documentStream = request.Document.OpenReadStream();

        try
        {
            var account = await _mediator.Send(new AddDocumentCommand(
                accountId,
                documentStream,
                request.Document.FileName,
                request.Document.ContentType), cancellationToken);

            return Ok(account);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Lists the documents attached to an account.</summary>
    /// <param name="accountId">The account identifier.</param>
    [HttpGet("{accountId:guid}/documents")]
    [EndpointSummary("Get account documents")]
    [ProducesResponseType(typeof(IReadOnlyList<DocumentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocuments(Guid accountId, CancellationToken cancellationToken)
    {
        try
        {
            var documents = await _mediator.Send(new GetAccountDocumentsQuery(accountId), cancellationToken);
            return Ok(documents);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Marks an account document as verified.</summary>
    /// <param name="accountId">The account identifier.</param>
    /// <param name="documentId">The document identifier.</param>
    [HttpPost("{accountId:guid}/documents/{documentId:guid}/verify")]
    [EndpointSummary("Verify document")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyDocument(Guid accountId, Guid documentId, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _mediator.Send(new VerifyDocumentCommand(accountId, documentId), cancellationToken);
            return Ok(account);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Marks an account document as rejected.</summary>
    /// <param name="accountId">The account identifier.</param>
    /// <param name="documentId">The document identifier.</param>
    [HttpPost("{accountId:guid}/documents/{documentId:guid}/reject")]
    [EndpointSummary("Reject document")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectDocument(Guid accountId, Guid documentId, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _mediator.Send(new RejectDocumentCommand(accountId, documentId), cancellationToken);
            return Ok(account);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    /// <summary>Closes an open account. Raises <c>AccountClosed</c>.</summary>
    /// <param name="accountId">The account identifier.</param>
    [HttpPost("{accountId:guid}/close")]
    [EndpointSummary("Close account")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close(Guid accountId, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _mediator.Send(new CloseAccountCommand(accountId), cancellationToken);
            return Ok(account);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
