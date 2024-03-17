using AutoMapper;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Contracts.Task.Response;

namespace TaskManagementSystem.Application.MappingProfiles;
public class TaskMapping : Profile
{
    public TaskMapping()
    {
        CreateMap<Domain.Tasks.Task, TaskWithCategoryDetailsResponse>()
            .ForMember
                (
                   dest => dest.TaskId,
                   opt => opt.MapFrom(src => src.Id)
                );

        CreateMap<Domain.Tasks.Task, CreateTask>();

        CreateMap<Domain.Tasks.Task, TaskWithCategoryDetailsResponse>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.User, opt => opt.MapFrom
                (
                    src => new UserObj
                    {
                        UserName = src.User.Email,
                        FullName = $"{src.User.FirstName} {src.User.LastName}"
                    }
                )
             );

        CreateMap<Domain.Tasks.Task, UpdateTask>();
    }
}
