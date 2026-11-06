using System.ComponentModel;
using BankService.Application.UseCases.Commands.OpenAccount;
using BankService.Models;
using MediatR;
using ModelContextProtocol.Server;

namespace BankService.Mcp;

/// <summary>
/// MCP tool surface for the banking bounded context. Like the REST controllers this is a thin
/// presentation adapter: it translates tool calls into application commands via MediatR and
/// contains no business logic. The document is passed as base64 because MCP tool calls are JSON.
/// </summary>
[McpServerToolType]
public sealed class AccountOpeningTools
{
    private readonly IMediator _mediator;

    public AccountOpeningTools(IMediator mediator)
    {
        _mediator = mediator;
    }

    [McpServerTool(Name = "open_bank_account"), Description(
        "Applies to open a bank account for a customer. The customer's credit score is checked " +
        "against the eligibility minimum (650); on approval the account is persisted, an " +
        "AccountOpened event is published and a welcome email is sent. Returns the approval " +
        "decision, message and account details.")]
    public async Task<OpenAccountResponse> OpenAccount(
        [Description("Full name of the customer applying for the account.")] string fullName,
        [Description("Base64-encoded content of the onboarding document (e.g. ID proof).")] string documentBase64,
        [Description("File name of the onboarding document, including extension (e.g. id-proof.pdf).")] string documentFileName,
        [Description("MIME type of the document (e.g. application/pdf, image/png).")] string documentContentType = "application/octet-stream",
        CancellationToken cancellationToken = default)
    {
        byte[] documentBytes;
        try
        {
            documentBytes = Convert.FromBase64String(documentBase64);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("documentBase64 is not valid base64 content.", nameof(documentBase64), ex);
        }

        await using var documentStream = new MemoryStream(documentBytes, writable: false);

        var command = new OpenAccountCommand(Guid.NewGuid(), fullName, documentStream, documentFileName, documentContentType);

        var result = await _mediator.Send(command, cancellationToken);

        return new OpenAccountResponse
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
    }
}
