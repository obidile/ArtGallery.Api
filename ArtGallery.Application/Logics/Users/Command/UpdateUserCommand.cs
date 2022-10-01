using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Handlers.Users.Commands;

public partial class UpdateUserCommand : IRequest<ResponseModel>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public string UpdatedBy { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MaximumLength(255);
        RuleFor(c => c.LastName).NotEmpty().MaximumLength(255);
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (user == null)
        {
            //throw new NotFoundException("User is not found.");
            return ResponseModel.Failure("User is not found");  //I'm to ask daddy Rhema about this
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.EmailAddress = request.EmailAddress;


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<UserModel>.Success(_mapper.Map<UserModel>(user), "User was successfully updated");
    }
}
