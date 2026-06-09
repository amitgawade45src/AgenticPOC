using BankService.Domain.Common;
using BankService.Domain.Enums;

namespace BankService.Domain.Entities;

/// <summary>
/// A child entity of the <see cref="BankAccount"/> aggregate representing an
/// onboarding document the customer uploaded. It has its own identity inside the
/// aggregate but no repository of its own: it is created and mutated only through
/// the <see cref="BankAccount"/> root, which is the aggregate's consistency boundary.
/// </summary>
public sealed class Document : Entity
{
    public string Uri { get; private set; }
    public string FileName { get; private set; }
    public DocumentVerificationStatus VerificationStatus { get; private set; }
    public DateTime UploadedOnUtc { get; private set; }

    private Document(
        Guid id,
        string uri,
        string fileName,
        DocumentVerificationStatus verificationStatus,
        DateTime uploadedOnUtc)
        : base(id)
    {
        Uri = uri;
        FileName = fileName;
        VerificationStatus = verificationStatus;
        UploadedOnUtc = uploadedOnUtc;
    }

    /// <summary>
    /// Creates a newly uploaded document in the <see cref="DocumentVerificationStatus.Pending"/>
    /// state. Internal so that only the aggregate root (in this assembly) can create one;
    /// callers attach documents via <see cref="BankAccount.AddDocument"/>.
    /// </summary>
    internal static Document Upload(string uri, string fileName, DateTime uploadedOnUtc) =>
        new(Guid.NewGuid(), uri, fileName, DocumentVerificationStatus.Pending, uploadedOnUtc);

    /// <summary>
    /// Reconstitutes a document from persisted state. Public so the infrastructure
    /// layer can rebuild the child when rehydrating the aggregate.
    /// </summary>
    public static Document Restore(
        Guid id,
        string uri,
        string fileName,
        DocumentVerificationStatus verificationStatus,
        DateTime uploadedOnUtc) =>
        new(id, uri, fileName, verificationStatus, uploadedOnUtc);

    // State transitions are internal: they may only be invoked by the aggregate root,
    // which enforces that the document belongs to the account before changing it.
    internal void MarkVerified() => VerificationStatus = DocumentVerificationStatus.Verified;

    internal void MarkRejected() => VerificationStatus = DocumentVerificationStatus.Rejected;
}
