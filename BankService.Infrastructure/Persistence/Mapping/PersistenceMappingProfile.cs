using AutoMapper;
using BankService.Domain.Entities;
using BankService.Domain.Enums;
using BankService.Domain.ValueObjects;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence.Mapping;

/// <summary>
/// AutoMapper profile that translates between the domain <see cref="BankAccount"/>
/// aggregate and the infrastructure <see cref="BankAccountRecord"/> persistence model,
/// including its <see cref="Document"/> child entities.
/// </summary>
public sealed class PersistenceMappingProfile : Profile
{
    public PersistenceMappingProfile()
    {
        // Domain -> persistence record: flatten the value objects to primitives.
        CreateMap<BankAccount, BankAccountRecord>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Value))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));

        CreateMap<Document, DocumentRecord>()
            .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => src.VerificationStatus.ToString()));

        // Persistence record -> domain: reconstitute the aggregate (and its child
        // documents) via the domain factories. ForAllMembers(Ignore) ensures
        // AutoMapper does not try to write to the aggregate's private setters.
        CreateMap<BankAccountRecord, BankAccount>()
            .ConstructUsing((src, ctx) => BankAccount.Restore(
                src.Id,
                CustomerId.From(src.CustomerId),
                AccountNumber.From(src.AccountNumber),
                src.EarningScore,
                Enum.Parse<AccountStatus>(src.Status),
                src.Documents.Select(d => Document.Restore(
                    d.Id,
                    d.Uri,
                    d.FileName,
                    Enum.Parse<DocumentVerificationStatus>(d.VerificationStatus),
                    d.UploadedOnUtc)),
                src.CreatedOnUtc))
            .ForAllMembers(opt => opt.Ignore());
    }
}
