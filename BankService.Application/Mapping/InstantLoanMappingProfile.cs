using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;

namespace BankService.Application.Mapping;

/// <summary>
/// AutoMapper profile for projecting <see cref="InstantLoan"/> to <see cref="InstantLoanDto"/>.
/// </summary>
public sealed class InstantLoanMappingProfile : Profile
{
    public InstantLoanMappingProfile()
    {
        CreateMap<InstantLoan, InstantLoanDto>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
