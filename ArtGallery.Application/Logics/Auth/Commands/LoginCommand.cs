using ArtGallery.Application.Common.Enums;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using CryptoHelper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArtGallery.Application.Logics.Auth.Commands;

public class LoginCommand : IRequest<ResponseModel>
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.EmailAddress)
            .MaximumLength(255)
            .NotEmpty();

        RuleFor(v => v.Password)
           .MaximumLength(255)
           .NotEmpty();
    }
}
public class LoginCommandHandler : IRequestHandler<LoginCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public LoginCommandHandler(IApplicationContext dbContext, IMapper mapper, IConfiguration config, IMediator mediator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mediator = mediator;
        _config = config;
    }

    public async Task<ResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Where(x => x.EmailAddress.ToLower() == request.EmailAddress.ToLower() || x.PhoneNumber == request.EmailAddress).FirstOrDefaultAsync();

        if (user == null)
        {
            return ResponseModel.Failure("Wrong EmailAddress or Password. Please try again.");
        }

        if (!user.IsActive)
        {
            return ResponseModel.Failure("Your account is not active. Please contact our contact center.");
        }

        //if (user.IsBlocked == true)
        //{
        //    return ResponseModel.Failure("Your account has been suspended. Please contact our contact center.");
        //    //return ResponseModel.Failure("Your profile has been disabled by admin. Please contact the support.");
        //}

        var verified = Crypto.VerifyHashedPassword(user.PasswordHash, request.Password);

        if (!verified)
        {
            return ResponseModel.Failure($"Invalid password.");
        }

        //if (user.AccountType == AccountTypeEnum.Admin && request.LoginChannel != "Admin")
        //{
        //    return ResponseModel.Failure("You're not authorized to login here.");
        //}
        //else if (user.AccountType == AccountType.Customer && request.LoginChannel == "Admin")
        //{
        //    return ResponseModel.Failure("You're not authorized to login here.");
        //}

        var jwtToken = GenerateToken(user);

        // Reset failed count
        //user.LastLoginDate = DateTime.Now;
        //_dbContext.Users.Update(user);
        //await _dbContext.SaveChangesAsync();

        //return ResponseModel<LoginResponseModel>.Success(response, "Login was successful.");
        return ResponseModel.Success("Login was successful.");
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("Jwt:SecretKey"));

        var expiryTime = _config.GetValue<int>("Jwt:ExpiryTime");

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim("userId", user.Id.ToString()),
            new Claim("name", $"{user.FirstName} {user.LastName}"),
            new Claim("email", user.EmailAddress),
        };

        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            claims.Add(new Claim("phoneNumber", user.PhoneNumber));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryTime),
            Issuer = _config.GetValue<string>("Jwt:Issuer"),
            Audience = _config.GetValue<string>("Jwt:Audience"),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}