namespace BankService.Domain.Common;

/// <summary>
/// Marker interface identifying an aggregate root: the single entry point into an
/// aggregate and the only type a repository may load or persist. Use it to
/// constrain repositories so they can operate on aggregate roots only.
/// </summary>
public interface IAggregateRoot
{
}
