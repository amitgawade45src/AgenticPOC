using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using MediatR;

namespace BankService.Application.UseCases.Queries.Lending;

public sealed record GetHomeLoanDetailsQuery(Guid LoanId) : IRequest<HomeLoanDto?>;

public sealed class GetHomeLoanDetailsQueryHandler : IRequestHandler<GetHomeLoanDetailsQuery, HomeLoanDto?>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IMapper _mapper;

    public GetHomeLoanDetailsQueryHandler(IHomeLoanRepository homeLoanRepository, IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _mapper = mapper;
    }

    public async Task<HomeLoanDto?> Handle(GetHomeLoanDetailsQuery request, CancellationToken cancellationToken)
    {
        var loan = await _homeLoanRepository.GetByIdAsync(request.LoanId, cancellationToken);
        return loan is null ? null : _mapper.Map<HomeLoanDto>(loan);
    }
}

public sealed record GetOutstandingAmountQuery(Guid LoanId) : IRequest<decimal?>;

public sealed class GetOutstandingAmountQueryHandler : IRequestHandler<GetOutstandingAmountQuery, decimal?>
{
    private readonly IHomeLoanRepository _homeLoanRepository;

    public GetOutstandingAmountQueryHandler(IHomeLoanRepository homeLoanRepository)
    {
        _homeLoanRepository = homeLoanRepository;
    }

    public async Task<decimal?> Handle(GetOutstandingAmountQuery request, CancellationToken cancellationToken)
    {
        var loan = await _homeLoanRepository.GetByIdAsync(request.LoanId, cancellationToken);
        return loan?.OutstandingAmount;
    }
}

public sealed record GetCustomerHomeLoansQuery(Guid CustomerId) : IRequest<IReadOnlyList<HomeLoanDto>>;

public sealed class GetCustomerHomeLoansQueryHandler : IRequestHandler<GetCustomerHomeLoansQuery, IReadOnlyList<HomeLoanDto>>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IMapper _mapper;

    public GetCustomerHomeLoansQueryHandler(IHomeLoanRepository homeLoanRepository, IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<HomeLoanDto>> Handle(GetCustomerHomeLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _homeLoanRepository.GetByCustomerIdAsync(CustomerId.From(request.CustomerId), cancellationToken);
        return _mapper.Map<IReadOnlyList<HomeLoanDto>>(loans);
    }
}
