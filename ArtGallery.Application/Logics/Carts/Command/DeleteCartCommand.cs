using MediatR;
using ArtGallery.Application.Common.Interfaces;
using FluentValidation;
using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Handlers.Carts.Commands;

public class DeleteCartCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}

public class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteCartCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<ResponseModel> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Carts.FindAsync(request.Id);

        if (entity == null)
        {
            return ResponseModel.Failure("Cart Was not found");
        }

        //entity.LastModifiedBy = request.DeletedBy;
        _dbContext.Carts.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("Cart was deleted Successfully");
    }
}
