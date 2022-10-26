using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Application.Logics.Categories;

public class CreateCategoryCommand : IRequest<ResponseModel>
{
    public string Name { get; set; }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).Empty();
    }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateCategoryCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.Categories.AsNoTracking().AnyAsync(x => x.Name == request.Name);
        if (exist)
        {
            return ResponseModel.Failure("This Category name already exist.");
        }

        var model = new Category
        {
            Name = request.Name
        };

        _dbContext.Categories.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<CategoryModel>.Success(_mapper.Map<CategoryModel>(model), "Category Name Was Successfully Added");
    }
}
