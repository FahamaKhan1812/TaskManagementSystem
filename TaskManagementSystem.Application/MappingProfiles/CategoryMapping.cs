using AutoMapper;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Contracts.Category.Response;
using TaskManagementSystem.Domain.Categories;

namespace TaskManagementSystem.Application.MappingProfiles;
public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryResponse>();
        CreateMap<Category, CreateCategory>();
        CreateMap<Category, UpdateCategory>();
    }
}
