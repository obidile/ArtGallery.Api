using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using ArtGallery.Domain.Enums;
using AutoMapper;
using CryptoHelper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Application.Logics.Users;

public class CreateUserCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public AccountTypeEnum AccountType { get; set; }
    public string CreatedBy { get; set; }
} 

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(v => v.LastName).NotEmpty().MaximumLength(255);
        RuleFor(v => v.Password).NotEmpty().MaximumLength(30);
        RuleFor(v => v.PhoneNumber).NotEmpty().MaximumLength(30);
        RuleFor(v => v.EmailAddress)
        .MaximumLength(255)
        .NotEmpty()
        .EmailAddress().WithMessage("Enter a valid email address");
        RuleFor(v => v.Password).NotEmpty().MaximumLength(30);
        RuleFor(v => v.Location).NotEmpty();
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateUserCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.Users.AsNoTracking().AnyAsync(x => x.EmailAddress == request.EmailAddress);
        if (exist)
        {
            return ResponseModel.Failure("The specified email address already exists.");
        }

        var model = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailAddress = request.EmailAddress,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            IsActive = true,
            AccountType = request.AccountType,
            PasswordHash = Crypto.HashPassword(request.Password)
        };

        _dbContext.Users.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<UserModel>.Success(_mapper.Map<UserModel>(model), "User successfully created");
    }
}
