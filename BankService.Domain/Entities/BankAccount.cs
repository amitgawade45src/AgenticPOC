using BankService.Domain.Common;
using BankService.Domain.Enums;
using BankService.Domain.Events;
using BankService.Domain.Exceptions;
using BankService.Domain.Rules;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Entities;

/// <summary>
/// The aggregate root for the account-opening process. It owns the invariants of
/// the account lifecycle: an account can only be opened for an eligible customer,
/// and opening it raises an <see cref="AccountOpenedEvent"/>.
/// </summary>
public sealed class BankAccount : AggregateRoot
{
    private readonly List<Document> _documents = new();

    public CustomerId CustomerId { get; private set; }
    public AccountNumber AccountNumber { get; private set; }
    public int EarningScore { get; private set; }
    public AccountStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    /// <summary>
    /// The onboarding documents attached to this account. Exposed read-only: callers
    /// add documents through <see cref="AddDocument"/> so the root stays the single
    /// entry point into the aggregate.
    /// </summary>
    public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();

    // Used by the Restore factory to reconstitute a persisted aggregate.
    private BankAccount()
    {
    }

    private BankAccount(
        CustomerId customerId,
        AccountNumber accountNumber,
        int earningScore,
        DateTime createdOnUtc)
        : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        AccountNumber = accountNumber;
        EarningScore = earningScore;
        CreatedOnUtc = createdOnUtc;
        Status = AccountStatus.Open;
    }

    /// <summary>
    /// Factory that enforces the core domain invariant: an account may only be
    /// opened when the customer satisfies <see cref="AccountEligibilityRule"/>.
    /// </summary>
    /// <exception cref="IneligibleCustomerException">
    /// Thrown when the earning score is below the required minimum.
    /// </exception>
    public static BankAccount Open(
        CustomerId customerId,
        int earningScore,
        string documentUri,
        string documentFileName,
        DateTime createdOnUtc)
    {
        if (!AccountEligibilityRule.IsEligible(earningScore))
        {
            throw new IneligibleCustomerException(earningScore, AccountEligibilityRule.MinimumEarningScore);
        }

        var account = new BankAccount(
            customerId,
            AccountNumber.Generate(),
            earningScore,
            createdOnUtc);

        account.AddDocument(documentUri, documentFileName, createdOnUtc);
        account.RaiseDomainEvent(new AccountOpenedEvent(customerId, account.AccountNumber, createdOnUtc));
        return account;
    }

    /// <summary>
    /// Attaches a newly uploaded document to the account. The child entity is created
    /// only here, through the root, so the aggregate controls its own contents.
    /// </summary>
    public Document AddDocument(string uri, string fileName, DateTime uploadedOnUtc)
    {
        var document = Document.Upload(uri, fileName, uploadedOnUtc);
        _documents.Add(document);
        return document;
    }

    /// <summary>
    /// Marks one of the account's documents as verified. The root enforces the
    /// invariant that the document must belong to this aggregate before mutating it.
    /// </summary>
    public void VerifyDocument(Guid documentId) => RequireDocument(documentId).MarkVerified();

    /// <summary>
    /// Marks one of the account's documents as rejected.
    /// </summary>
    public void RejectDocument(Guid documentId) => RequireDocument(documentId).MarkRejected();

    private Document RequireDocument(Guid documentId)
    {
        var document = _documents.FirstOrDefault(d => d.Id == documentId);
        if (document is null)
        {
            throw new InvalidOperationException(
                $"Document {documentId} does not belong to account {Id}.");
        }

        return document;
    }

    /// <summary>
    /// Reconstitutes an existing account from persisted state. Unlike <see cref="Open"/>,
    /// this does not re-run creation rules or raise domain events; it simply rebuilds
    /// the aggregate as it was stored. Intended for use by the infrastructure layer.
    /// </summary>
    public static BankAccount Restore(
        Guid id,
        CustomerId customerId,
        AccountNumber accountNumber,
        int earningScore,
        AccountStatus status,
        IEnumerable<Document> documents,
        DateTime createdOnUtc)
    {
        var account = new BankAccount
        {
            Id = id,
            CustomerId = customerId,
            AccountNumber = accountNumber,
            EarningScore = earningScore,
            Status = status,
            CreatedOnUtc = createdOnUtc
        };

        account._documents.AddRange(documents);
        return account;
    }
}
