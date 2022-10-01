using MediatR;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Common.Interfaces;
using FluentValidation;

namespace ArtGallery.Application.Logics.Orders.Command;

public class DeleteOrderCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteOrderCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<ResponseModel> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Orders.FindAsync(request.Id);

        if (entity == null)
        {
            return ResponseModel.Failure("Order Was not found");
        }

        //entity.LastModifiedBy = request.DeletedBy;
        _dbContext.Orders.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("Order was deleted Successfully");
    }
}
