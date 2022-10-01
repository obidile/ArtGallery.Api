using MediatR;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Handlers.Users.Commands;

public class DeleteUserCommand : IRequest<ResponseModel>
{
    public long UserId { get; set; }
    //public string DeletedBy { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;

    public DeleteUserCommandHandler(IApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task<ResponseModel> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users.FindAsync(request.UserId);

        if (entity == null)
        {
            //throw new NotFoundException("User was not found");
            return ResponseModel.Failure("User Was not found"); //I'm to ask daddy Rhema about this
        }

        //entity.LastModifiedBy = request.DeletedBy;
        _dbContext.Users.Remove(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel.Success("User was deleted Successfully");
    }
}
