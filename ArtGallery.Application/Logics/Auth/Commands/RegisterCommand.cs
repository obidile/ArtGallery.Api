using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
//using Microsoft.AspNetCore.Http;

namespace ArtGallery.Application.Logics.Auth.Commands;

public class RegisterCommand : IRequest<ResponseModel>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(c => c.LastName).NotEmpty().MaximumLength(255);
        RuleFor(c => c.EmailAddress).NotEmpty().EmailAddress();
        RuleFor(c => c.PhoneNumber).NotEmpty();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(6).MaximumLength(26);
        RuleFor(c => c.ConfirmPassword).Equal(c => c.Password).When(c => !string.IsNullOrWhiteSpace(c.Password)).WithMessage("Confirm Password does not match.");
    }
}
public class PassWordValidator : AbstractValidator<string>
{
    public PassWordValidator()
    {
        RuleFor(p => p).NotEmpty().WithMessage("Your password cannot be empty")
            .MinimumLength(6).WithMessage("Your password length must be at least 6.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            //.Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).")
            ;
    }
}
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    //private readonly IHttpContextAccessor _httpContext;

    public RegisterCommandHandler(IApplicationContext dbContext, /* IHttpContextAccessor httpContext */ IMapper mapper)
    {
        _dbContext = dbContext;
        //_httpContext = httpContext;
        _mapper = mapper;
    }

    public Task<ResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}