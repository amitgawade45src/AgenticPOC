using AutoMapper;
using BankService.Application.DTOs;
using BankService.Application.Interfaces;
using BankService.Application.UseCases.Common;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using MediatR;

namespace BankService.Application.UseCases.Commands.CustomerManagement;

public sealed record RegisterCustomerCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string MobileNumber,
    string Address) : IRequest<CustomerDto>;

public sealed class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public RegisterCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Register(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.MobileNumber,
            request.Address,
            _dateTimeService.UtcNow);

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(customer, _publisher, cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }
}

public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string MobileNumber,
    string Address) : IRequest<CustomerDto>;

public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByCustomerIdAsync(CustomerId.From(request.CustomerId), cancellationToken)
            ?? throw new InvalidOperationException($"Customer {request.CustomerId} was not found.");

        customer.Update(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Email,
            request.MobileNumber,
            request.Address,
            _dateTimeService.UtcNow);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(customer, _publisher, cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }
}

public sealed record DeactivateCustomerCommand(Guid CustomerId) : IRequest<CustomerDto>;

public sealed class DeactivateCustomerCommandHandler : IRequestHandler<DeactivateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public DeactivateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByCustomerIdAsync(CustomerId.From(request.CustomerId), cancellationToken)
            ?? throw new InvalidOperationException($"Customer {request.CustomerId} was not found.");

        customer.Deactivate(_dateTimeService.UtcNow);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(customer, _publisher, cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }
}
