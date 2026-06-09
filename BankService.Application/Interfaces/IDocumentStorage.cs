namespace BankService.Application.Interfaces;

/// <summary>
/// Port for persisting an uploaded onboarding document. The implementation
/// (Azure Blob Storage) lives in the infrastructure layer.
/// </summary>
public interface IDocumentStorage
{
    /// <summary>
    /// Stores the supplied document stream and returns the absolute URI of the
    /// stored blob.
    /// </summary>
    Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
}
