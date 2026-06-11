using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.UseCases.Queries.GetAccount;

/// <summary>
/// Handles <see cref="GetAccountQuery"/> by loading the aggregate from the
/// repository and projecting it to an <see cref="AccountDto"/> read model.
/// </summary>
public sealed class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto?>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto?> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken);
        return account is null ? null : _mapper.Map<AccountDto>(account);
    }
}
