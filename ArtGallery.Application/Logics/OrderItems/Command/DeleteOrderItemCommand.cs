using MediatR;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Common.Interfaces;
using FluentValidation;
//using ArtGallery.Application.Logics.OrderItems.Command;

namespace ArtGallery.Application.Logics.OrderItems.Command;

public class DeleteOrderItemCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}

public class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
{
    public DeleteOrderItemCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteOrderItemCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<ResponseModel> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.OrderItems.FindAsync(request.Id);

        if (entity == null)
        {
            return ResponseModel.Failure("OrderItem Was not found");
        }

        //entity.LastModifiedBy = request.DeletedBy;
        _dbContext.OrderItems.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("OrderItem was deleted Successfully");
    }
}
