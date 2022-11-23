using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace ArtGallery.Application.Logics.CartItems.Command;

public class DeleteCartItemCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}
public class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
{
	public DeleteCartItemCommandValidator()
	{
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteCartItemCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }
    public async Task<ResponseModel> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CartItems.FindAsync(request.Id);

        if (entity == null)
        {
            return ResponseModel.Failure("Items Was not found");
        }

        _dbContext.CartItems.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("Deleted Successfully");
    }
}