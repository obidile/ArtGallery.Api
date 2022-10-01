using MediatR;
using ArtGallery.Application.Common.Interfaces;
using FluentValidation;
using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Handlers.Categories.Commands;

public class DeleteCategoryCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteCategoryCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<ResponseModel> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Categories.FindAsync(request.Id);

        if (entity == null)
        {
            return ResponseModel.Failure("Category Was not found");
        }

        //entity.LastModifiedBy = request.DeletedBy;
        _dbContext.Categories.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("Category was deleted Successfully");
    }
}
