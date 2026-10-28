using AutoMapper;
using BankService.Application.DTOs;
using BankService.Application.Interfaces;
using BankService.Application.UseCases.Common;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using MediatR;

namespace BankService.Application.UseCases.Commands.Lending;

public sealed record ApplyForInstantLoanCommand(Guid CustomerId, decimal RequestedAmount) : IRequest<InstantLoanDto>;

public sealed class ApplyForInstantLoanCommandHandler : IRequestHandler<ApplyForInstantLoanCommand, InstantLoanDto>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public ApplyForInstantLoanCommandHandler(
        IInstantLoanRepository instantLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<InstantLoanDto> Handle(ApplyForInstantLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = InstantLoan.Apply(CustomerId.From(request.CustomerId), request.RequestedAmount, _dateTimeService.UtcNow);

        await _instantLoanRepository.AddAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<InstantLoanDto>(loan);
    }
}

public sealed record ApproveInstantLoanCommand(Guid LoanId, decimal ApprovedAmount) : IRequest<InstantLoanDto>;

public sealed class ApproveInstantLoanCommandHandler : IRequestHandler<ApproveInstantLoanCommand, InstantLoanDto>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public ApproveInstantLoanCommandHandler(
        IInstantLoanRepository instantLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<InstantLoanDto> Handle(ApproveInstantLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _instantLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Instant loan {request.LoanId} was not found.");

        loan.Approve(request.ApprovedAmount, _dateTimeService.UtcNow);

        await _instantLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<InstantLoanDto>(loan);
    }
}

public sealed record RejectInstantLoanCommand(Guid LoanId) : IRequest<InstantLoanDto>;

public sealed class RejectInstantLoanCommandHandler : IRequestHandler<RejectInstantLoanCommand, InstantLoanDto>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public RejectInstantLoanCommandHandler(
        IInstantLoanRepository instantLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<InstantLoanDto> Handle(RejectInstantLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _instantLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Instant loan {request.LoanId} was not found.");

        loan.Reject(_dateTimeService.UtcNow);

        await _instantLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<InstantLoanDto>(loan);
    }
}

public sealed record DisburseInstantLoanCommand(Guid LoanId) : IRequest<InstantLoanDto>;

public sealed class DisburseInstantLoanCommandHandler : IRequestHandler<DisburseInstantLoanCommand, InstantLoanDto>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public DisburseInstantLoanCommandHandler(
        IInstantLoanRepository instantLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<InstantLoanDto> Handle(DisburseInstantLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _instantLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Instant loan {request.LoanId} was not found.");

        loan.Disburse(_dateTimeService.UtcNow);

        await _instantLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<InstantLoanDto>(loan);
    }
}

public sealed record RepayInstantLoanCommand(Guid LoanId, decimal Amount) : IRequest<InstantLoanDto>;

public sealed class RepayInstantLoanCommandHandler : IRequestHandler<RepayInstantLoanCommand, InstantLoanDto>
{
    private readonly IInstantLoanRepository _instantLoanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public RepayInstantLoanCommandHandler(
        IInstantLoanRepository instantLoanRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IPublisher publisher,
        IMapper mapper)
    {
        _instantLoanRepository = instantLoanRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task<InstantLoanDto> Handle(RepayInstantLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _instantLoanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new InvalidOperationException($"Instant loan {request.LoanId} was not found.");

        loan.Repay(request.Amount, _dateTimeService.UtcNow);

        await _instantLoanRepository.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await DomainEventPublisher.PublishAndClearAsync(loan, _publisher, cancellationToken);

        return _mapper.Map<InstantLoanDto>(loan);
    }
}
