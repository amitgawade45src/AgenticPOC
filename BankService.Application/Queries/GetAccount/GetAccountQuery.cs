using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Queries.GetAccount;

/// <summary>
/// Query to retrieve a single account by its identifier.
/// </summary>
public sealed record GetAccountQuery(Guid AccountId) : IRequest<AccountDto?>;
