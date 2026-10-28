using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;

namespace BankService.Application.Mapping;

/// <summary>
/// AutoMapper profile for projecting <see cref="HomeLoan"/> to <see cref="HomeLoanDto"/>.
/// </summary>
public sealed class HomeLoanMappingProfile : Profile
{
    public HomeLoanMappingProfile()
    {
        CreateMap<HomeLoan, HomeLoanDto>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
