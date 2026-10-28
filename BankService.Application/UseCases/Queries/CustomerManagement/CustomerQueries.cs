using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using MediatR;

namespace BankService.Application.UseCases.Queries.CustomerManagement;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDto?>;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByCustomerIdAsync(CustomerId.From(request.CustomerId), cancellationToken);
        return customer is null ? null : _mapper.Map<CustomerDto>(customer);
    }
}

public sealed record SearchCustomersQuery(string SearchTerm) : IRequest<IReadOnlyList<CustomerDto>>;

public sealed class SearchCustomersQueryHandler : IRequestHandler<SearchCustomersQuery, IReadOnlyList<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public SearchCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CustomerDto>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.SearchAsync(request.SearchTerm, cancellationToken);
        return _mapper.Map<IReadOnlyList<CustomerDto>>(customers);
    }
}

public sealed record GetCustomerProfileQuery(Guid CustomerId) : IRequest<CustomerDto?>;

public sealed class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerProfileQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByCustomerIdAsync(CustomerId.From(request.CustomerId), cancellationToken);
        return customer is null ? null : _mapper.Map<CustomerDto>(customer);
    }
}
