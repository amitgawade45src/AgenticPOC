using AutoMapper;
using BankService.Application.DTOs;
using BankService.Application.Interfaces;
using BankService.Application.UseCases.Common;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using MediatR;

namespace BankService.Application.UseCases.Commands.Lending;

public sealed record ApplyForHomeLoanCommand(
    Guid CustomerId,
    Guid AccountId,
    string PropertyAddress,
    decimal LoanAmount,
    decimal InterestRate,
    int LoanTermMonths) : IRequest<HomeLoanDto>;

public sealed class ApplyForHomeLoanCommandHandler : IRequestHandler<ApplyForHomeLoanCommand, HomeLoanDto>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public ApplyForHomeLoanCommandHandler(
        IHomeLoanRepository homeLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<HomeLoanDto> Handle(ApplyForHomeLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = HomeLoan.Apply(
            CustomerId.From(request.CustomerId),
            request.AccountId,
            request.PropertyAddress,
            request.LoanAmount,
            request.InterestRate,
            request.LoanTermMonths,
            _dateTimeService.UtcNow);

        await _homeLoanRepository.AddAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<HomeLoanDto>(loan);
    }
}

public sealed record ApproveHomeLoanCommand(Guid LoanId) : IRequest<HomeLoanDto>;

public sealed class ApproveHomeLoanCommandHandler : IRequestHandler<ApproveHomeLoanCommand, HomeLoanDto>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public ApproveHomeLoanCommandHandler(
        IHomeLoanRepository homeLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<HomeLoanDto> Handle(ApproveHomeLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _homeLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Home loan {request.LoanId} was not found.");

        loan.Approve(_dateTimeService.UtcNow);

        await _homeLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<HomeLoanDto>(loan);
    }
}

public sealed record RejectHomeLoanCommand(Guid LoanId) : IRequest<HomeLoanDto>;

public sealed class RejectHomeLoanCommandHandler : IRequestHandler<RejectHomeLoanCommand, HomeLoanDto>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public RejectHomeLoanCommandHandler(
        IHomeLoanRepository homeLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<HomeLoanDto> Handle(RejectHomeLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _homeLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Home loan {request.LoanId} was not found.");

        loan.Reject(_dateTimeService.UtcNow);

        await _homeLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<HomeLoanDto>(loan);
    }
}

public sealed record DisburseHomeLoanCommand(Guid LoanId) : IRequest<HomeLoanDto>;

public sealed class DisburseHomeLoanCommandHandler : IRequestHandler<DisburseHomeLoanCommand, HomeLoanDto>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public DisburseHomeLoanCommandHandler(
        IHomeLoanRepository homeLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<HomeLoanDto> Handle(DisburseHomeLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _homeLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Home loan {request.LoanId} was not found.");

        loan.Disburse(_dateTimeService.UtcNow);

        await _homeLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<HomeLoanDto>(loan);
    }
}

public sealed record PayEmiCommand(Guid LoanId, decimal Amount) : IRequest<HomeLoanDto>;

public sealed class PayEmiCommandHandler : IRequestHandler<PayEmiCommand, HomeLoanDto>
{
    private readonly IHomeLoanRepository _homeLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public PayEmiCommandHandler(
        IHomeLoanRepository homeLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _homeLoanRepository = homeLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<HomeLoanDto> Handle(PayEmiCommand request, CancellationToken cancellationToken)
    {
        var loan = await _homeLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Home loan {request.LoanId} was not found.");

        loan.PayEmi(request.Amount, _dateTimeService.UtcNow);

        await _homeLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<HomeLoanDto>(loan);
    }
}
