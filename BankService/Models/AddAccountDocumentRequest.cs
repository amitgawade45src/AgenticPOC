namespace BankService.Models;

/// <summary>
/// Request bound from a multipart form to attach a document to an existing account.
/// </summary>
public sealed class AddAccountDocumentRequest
{
    public IFormFile Document { get; set; } = default!;
}
