using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Commands.OpenAccount;

/// <summary>
/// Command representing a customer's request to open a bank account, dispatched
/// from the presentation layer when "Apply to open account" is clicked.
/// The uploaded document is carried as a stream plus its metadata.
/// </summary>
public sealed record OpenAccountCommand(Guid CustomerId, string FullName, Stream DocumentContent, string DocumentFileName, string DocumentContentType) : IRequest<AccountOpeningResult>;
