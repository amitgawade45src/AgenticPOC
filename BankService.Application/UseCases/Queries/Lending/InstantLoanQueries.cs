using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using MediatR;

namespace BankService.Application.UseCases.Queries.Lending;

public sealed record GetInstantLoanDetailsQuery(Guid LoanId) : IRequest<InstantLoanDto?>;

public sealed class GetInstantLoanDetailsQueryHandler : IRequestHandler<GetInstantLoanDetailsQuery, InstantLoanDto?>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IMapper _mapper;

    public GetInstantLoanDetailsQueryHandler(IInstantLoanRepository instantLoanRepository, IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _mapper = mapper;
    }

    public async Task<InstantLoanDto?> Handle(GetInstantLoanDetailsQuery request, CancellationToken cancellationToken)
    {
        var loan = await _instantLoanRepository.GetByIdAsync(request.LoanId, cancellationToken);
        return loan is null ? null : _mapper.Map<InstantLoanDto>(loan);
    }
}

public sealed record GetCustomerInstantLoansQuery(Guid CustomerId) : IRequest<IReadOnlyList<InstantLoanDto>>;

public sealed class GetCustomerInstantLoansQueryHandler : IRequestHandler<GetCustomerInstantLoansQuery, IReadOnlyList<InstantLoanDto>>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IMapper _mapper;

    public GetCustomerInstantLoansQueryHandler(IInstantLoanRepository instantLoanRepository, IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<InstantLoanDto>> Handle(GetCustomerInstantLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _instantLoanRepository.GetByCustomerIdAsync(CustomerId.From(request.CustomerId), cancellationToken);
        return _mapper.Map<IReadOnlyList<InstantLoanDto>>(loans);
    }
}
