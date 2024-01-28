﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Categories.CommandHandlers;
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, OperationResult<CreateCategory>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(DataContext dataContext,
                                        IMapper mapper,
                                        ILogger<CreateCategoryCommandHandler> logger)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OperationResult<CreateCategory>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CreateCategory>();
        try
        {
            Category category = new()
            {
                Id = request.Id,
                Name = request.Name,
            };
            if (request.UserRole == UserRole.User)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.UserNotAllowed,
                    Message = "User is not allowed to perform this action"
                };
                result.Errors.Add(error);
                return result;
            }
            await _dataContext.Categories.AddAsync(category, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            var mappedCategory = _mapper.Map<CreateCategory>(category);

            result.Payload = mappedCategory;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error erros = new()
            {
                Code = ErrorCode.UnknownError,
                Message = ex.Message
            };

            result.Errors.Add(erros);
            // Log the exception details
            _logger.LogError(ex, $"An error occurred while creating category: {erros}");
        }

        return result;
    }
}