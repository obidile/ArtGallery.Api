using ArtGallery.Application.Common.Helpers;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ArtGallery.Application.Logics.Customers.Command;

public class CreateCustomerCommand : IRequest<string>
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public string phoneNumber { get; set; }
    public DateTime dateOfBirth { get; set; }
}
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.firstName).NotEmpty();
        RuleFor(x => x.lastName).NotEmpty();
        RuleFor(x => x.mailAddress).EmailAddress();
        RuleFor(x => x.phoneNumber).NotEmpty();
    }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public CreateCustomerCommandHandler(IApplicationContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Check if the email already exists and return appropriate message.
        if (_dbContext.Customers.Any(x => x.mailAddress.ToLower() == request.mailAddress))
        { 
            return "This mail address already exists";
        }

        //// Check if the phoneNumber already exists and return appropriate message.
        if (_dbContext.Customers.Any(x => x.phoneNumber == request.phoneNumber))
        {
            return "This Phone number already exist";
        }

        // Check if the Name contains "Samuel" and return an error that Samuel is not allowed.
        var disAllowedName = "samuel";
        if (request.firstName.ToLower() == disAllowedName || request.lastName.ToLower() == disAllowedName)
        {
            return "The name Samuel Isn't allowed";
        }

        // create a blacklist parameter in appSettings and ensure that any name that exist in your blacklist is not allowed in application.
        var blackNames = _configuration.GetSection("BlackList").Get<List<string>>();
        if (blackNames.Any(x => x.ToLower() == request.firstName.ToLower() || x.ToLower() == request.lastName.ToLower()))
            // x.Contains(request.firstName)
        {
            return "The name you inputed isn't allowed";
        }

        //Ensure that the customer is x years and above
        //The value of x must be from the config file(appSettings.json)
        var minimumAge = _configuration.GetValue<int>("MinimumAge");
        var age = AgeHelper.GetAge(request.dateOfBirth);
        if (age < minimumAge)
        {
            return "The required Minimum age is 18 and above";
        }
        

    var model = new Customer()
        {
            firstName = request.firstName,
            lastName = request.lastName,
            mailAddress = request.mailAddress,
            phoneNumber = request.phoneNumber,
        };

        _dbContext.Customers.Add(model);

        await _dbContext.SaveChangesAsync();
        return "Customer created successfully";
    }
}