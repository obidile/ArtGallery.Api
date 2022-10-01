using MediatR;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Common.Interfaces;
using FluentValidation;

namespace ArtGallery.Application.Logics.ArtWorks.Command;

public class DeleteArtWorkCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}

public class DeleteArtWorkCommandValidator : AbstractValidator<DeleteArtWorkCommand>
{
    public DeleteArtWorkCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteArtWorkCommandHandler : IRequestHandler<DeleteArtWorkCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteArtWorkCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<ResponseModel> Handle(DeleteArtWorkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.ArtWorks.FindAsync(request.Id);

        if (entity == null)
        {
            return ResponseModel.Failure("ArtWork Was not found");
        }

        //entity.LastModifiedBy = request.DeletedBy;
        _dbContext.ArtWorks.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("ArtWork was deleted Successfully");
    }
}
