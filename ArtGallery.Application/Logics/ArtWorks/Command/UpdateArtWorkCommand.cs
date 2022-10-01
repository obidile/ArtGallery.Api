using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Handlers.ArtWorks.Commands;

public partial class UpdateArtWorkCommand : IRequest<ResponseModel>
{
    public long ArtWorkId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long Rating { get; set; }
    public long Quantity { get; set; }
    public long Price { get; set; }
    public Category Category { get; set; }
    public string ArtImage { get; set; }
    public long CategoryId { get; set; }
    public long DisCount { get; set; }
    public DateTime ProductionYear { get; set; }
}

public class UpdateArtWorkCommandValidator : AbstractValidator<UpdateArtWorkCommand>
{
    public UpdateArtWorkCommandValidator()
    {
        RuleFor(v => v.ArtWorkId).NotEmpty();
    }
}

public class UpdateArtWorkCommandHandler : IRequestHandler<UpdateArtWorkCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateArtWorkCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateArtWorkCommand request, CancellationToken cancellationToken)
    {
        var artWork = await _dbContext.ArtWorks.FirstOrDefaultAsync(x => x.Id == request.ArtWorkId);

        if (artWork == null)
        {
            return ResponseModel.Failure("ArtWork not found"); 
        }

        artWork.Title = request.Title;
        artWork.Description = request.Description;
        artWork.Rating = request.Rating;
        artWork.Quantity = request.Quantity;
        artWork.Price = request.Price;
        artWork.DisCount = request.DisCount;
        artWork.ProductionYear = request.ProductionYear;


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<ArtWorkModel>.Success(_mapper.Map<ArtWorkModel>(artWork), "ArtWork was successfully updated");
    }
}
