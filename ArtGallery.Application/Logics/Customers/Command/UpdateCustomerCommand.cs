using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Customers.Command;

public class UpdateCustomerCommand : IRequest<string>
{
    public long customerId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public string phoneNumber { get; set; }
    public DateTime dateOfBirth { get; set; }

}
public class UpdateCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public UpdateCustomerCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.Customers.AsNoTracking().AnyAsync(x => x.mailAddress == request.mailAddress || x.phoneNumber == request.phoneNumber);
        if (exist)
        {
            return "This Mail Address/phone number already exist";
        }

        if (request.firstName == "Samuel" || request.lastName == "Samuel")
        {
            return "The name Samuel Isn't allowed";
        }

        var customer = _dbContext.Customers.First(x => x.Id == request.customerId);

        customer.firstName = request.firstName;
        customer.lastName = request.lastName;
        customer.mailAddress = request.mailAddress;
        customer.phoneNumber = request.phoneNumber;

        await _dbContext.SaveChangesAsync();

        return "Customer updated successfully";
    }
}