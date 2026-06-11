using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.UseCases.Queries.GetAccount;

/// <summary>
/// Query to retrieve a single account by its identifier.
/// </summary>
public sealed record GetAccountQuery(Guid AccountId) : IRequest<AccountDto?>;
