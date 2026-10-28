using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;

namespace BankService.Application.Mapping;

/// <summary>
/// AutoMapper profile for projecting <see cref="Customer"/> to <see cref="CustomerDto"/>.
/// </summary>
public sealed class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
