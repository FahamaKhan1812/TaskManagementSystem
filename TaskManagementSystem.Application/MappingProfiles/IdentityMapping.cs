using AutoMapper;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.MappingProfiles;
internal class IdentityMapping : Profile
{
    public IdentityMapping()
    {
        CreateMap<User, IdentityResponse>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
             .ForMember(dest => dest.UserRole,
                opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.UserCreatedAt,
                opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.UserUpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedDate));
    }
}
