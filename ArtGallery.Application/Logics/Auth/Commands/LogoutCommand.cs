using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Auth.Commands;

public class LogoutCommand : IRequest<ResponseModel>
{
    public string RefreshToken { get; set; }
    public long UserId { get; set; }
}

public class LogoutCommandQueryValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandQueryValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty();
    }
}

public class LogoutCommandQueryHandler : IRequestHandler<LogoutCommand, ResponseModel>
{

    private readonly IApplicationContext _dbContext;

    public LogoutCommandQueryHandler(IApplicationContext context)
    {
        _dbContext = context;
    }
    public async Task<ResponseModel> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var saveChanges = false;
        //var user = await _dbContext.Users.AsNoTracking().Where(x => x.Id.Equals(request.UserId)).FirstOrDefaultAsync();
        //if (user != null)
        //{
        //    user.IsOnline = false;
        //    _dbContext.Users.Update(user);
        //}

        if (saveChanges)
        {
            await _dbContext.SaveChangesAsync();
        }

        return ResponseModel.Success("You have been logged out");
    }
}