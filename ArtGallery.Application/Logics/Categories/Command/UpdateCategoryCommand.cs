using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Handlers.Categories.Commands;

public partial class UpdateCategoryCommand : IRequest<ResponseModel>
{
    public long CategoryId { get; set; }
    public string Name { get; set; }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(v => v.CategoryId).NotEmpty();
    }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var Category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId);

        if (Category == null)
        {
            return ResponseModel.Failure("This Category was not found");
        }

        Category.Name = request.Name;


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<CategoryModel>.Success(_mapper.Map<CategoryModel>(Category), "Category name was successfully updated");
    }
}
