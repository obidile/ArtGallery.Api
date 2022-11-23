using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace ArtGallery.Application.Logics.Customers.Command;

public class DeleteCustomerCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
}
public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
	public DeleteCustomerCommandValidator()
	{
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    public DeleteCustomerCommandHandler(IApplicationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResponseModel> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = _dbContext.Customers.First(x => x.Id == request.Id);
        _dbContext.Customers.Remove(entity);
        await _dbContext.SaveChangesAsync();

        return ResponseModel.Success("Customer deleted Successfully");
    }
}
