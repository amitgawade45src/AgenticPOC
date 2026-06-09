namespace BankService.Domain.Enums;

/// <summary>
/// Verification state of an onboarding <see cref="Entities.Document"/> attached to a
/// <see cref="Entities.BankAccount"/>.
/// </summary>
public enum DocumentVerificationStatus
{
    /// <summary>Uploaded but not yet reviewed.</summary>
    Pending = 0,

    /// <summary>Reviewed and accepted.</summary>
    Verified = 1,

    /// <summary>Reviewed and rejected.</summary>
    Rejected = 2
}
