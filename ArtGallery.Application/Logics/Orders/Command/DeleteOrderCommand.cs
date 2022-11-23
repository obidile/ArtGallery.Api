using MediatR;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Common.Interfaces;
using FluentValidation;

namespace ArtGallery.Application.Logics.Orders.Command;

public class DeleteOrderCommand : IRequest<string>
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

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, string>
{
    private readonly IApplicationContext _dbContext;

    public DeleteOrderCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<string> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Orders.FindAsync(request.Id);

        if (entity == null)
        {
            return "Order Was not found";
        }

        _dbContext.Orders.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return "Order was deleted Successfully";
    }
}
