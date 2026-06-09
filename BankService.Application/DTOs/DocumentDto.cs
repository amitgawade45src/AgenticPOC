namespace BankService.Application.DTOs;

/// <summary>
/// Read model describing a document attached to a bank account.
/// </summary>
public sealed record DocumentDto
{
    public Guid Id { get; init; }
    public string Uri { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string VerificationStatus { get; init; } = string.Empty;
    public DateTime UploadedOnUtc { get; init; }
}
