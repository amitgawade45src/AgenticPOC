using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.UseCases.Queries.Banking;

public sealed record GetAccountDocumentsQuery(Guid AccountId) : IRequest<IReadOnlyList<DocumentDto>>;

public sealed class GetAccountDocumentsQueryHandler : IRequestHandler<GetAccountDocumentsQuery, IReadOnlyList<DocumentDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountDocumentsQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DocumentDto>> Handle(GetAccountDocumentsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Account {request.AccountId} was not found.");

        return _mapper.Map<IReadOnlyList<DocumentDto>>(account.Documents.ToList());
    }
}
