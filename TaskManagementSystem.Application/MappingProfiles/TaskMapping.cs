using AutoMapper;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Contracts.Task.Response;

namespace TaskManagementSystem.Application.MappingProfiles;
public class TaskMapping : Profile
{
    public TaskMapping()
    {
        CreateMap<Domain.Entities.Task, TaskWithCategoryDetailsResponse>()
            .ForMember
                (
                   dest => dest.TaskId,
                   opt => opt.MapFrom(src => src.Id)
                );

        CreateMap<Domain.Entities.Task, CreateTask>();
        
        CreateMap<Domain.Entities.Task, TaskWithCategoryDetailsResponse>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));

        CreateMap<Domain.Entities.Task, UpdateTask>();
    }
}
