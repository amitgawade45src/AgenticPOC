using AutoMapper;
using BankService.Application.DTOs;
using BankService.Application.Interfaces;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.UseCases.Commands.Banking;

public sealed record AddDocumentCommand(
    Guid AccountId,
    Stream DocumentContent,
    string DocumentFileName,
    string DocumentContentType) : IRequest<AccountDto>;

public sealed class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IDocumentStorage _documentStorage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMapper _mapper;

    public AddDocumentCommandHandler(
        IAccountRepository accountRepository,
        IDocumentStorage documentStorage,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _documentStorage = documentStorage;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Account {request.AccountId} was not found.");

        var documentUri = await _documentStorage.UploadAsync(
            request.DocumentContent,
            request.DocumentFileName,
            request.DocumentContentType,
            cancellationToken);

        account.AddDocument(documentUri, request.DocumentFileName, _dateTimeService.UtcNow);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountDto>(account);
    }
}

public sealed record VerifyDocumentCommand(Guid AccountId, Guid DocumentId) : IRequest<AccountDto>;

public sealed class VerifyDocumentCommandHandler : IRequestHandler<VerifyDocumentCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VerifyDocumentCommandHandler(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(VerifyDocumentCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Account {request.AccountId} was not found.");

        account.VerifyDocument(request.DocumentId);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountDto>(account);
    }
}

public sealed record RejectDocumentCommand(Guid AccountId, Guid DocumentId) : IRequest<AccountDto>;

public sealed class RejectDocumentCommandHandler : IRequestHandler<RejectDocumentCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RejectDocumentCommandHandler(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(RejectDocumentCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Account {request.AccountId} was not found.");

        account.RejectDocument(request.DocumentId);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountDto>(account);
    }
}

public sealed record CloseAccountCommand(Guid AccountId) : IRequest<AccountDto>;

public sealed class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMapper _mapper;

    public CloseAccountCommandHandler(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IDateTimeService dateTimeService,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new InvalidOperationException($"Account {request.AccountId} was not found.");

        account.Close(_dateTimeService.UtcNow);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountDto>(account);
    }
}
