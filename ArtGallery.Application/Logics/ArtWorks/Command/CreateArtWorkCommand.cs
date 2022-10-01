using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.ArtWorks.Command;

public class CreateArtWorkCommand : IRequest<ResponseModel>
{
    public long UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long Rating { get; set; }
    public long Quantity { get; set; }
    public long Price { get; set; }
    public long CategoryId { get; set; }
    public string ArtImage { get; set; }
    public long DisCount { get; set; }
    public DateTime ProductionYear { get; set; }
}

public class CreateArtWorkCommandValidator : AbstractValidator<CreateArtWorkCommand>
{
    public CreateArtWorkCommandValidator()
    {
        RuleFor(x => x.UserId).Empty();
        RuleFor(v => v.Title).NotEmpty().MaximumLength(255);
        RuleFor(v => v.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.Quantity).Empty();
        RuleFor(x => x.Price).Empty();
        RuleFor(x => x.ArtImage).Empty();
        RuleFor(x => x.DisCount).Empty();
        RuleFor(x => x.ProductionYear).Empty();


    }
}

public class CreateArtWorkCommandHandler : IRequestHandler<CreateArtWorkCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateArtWorkCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateArtWorkCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.ArtWorks.AsNoTracking().AnyAsync(x => x.Title == request.Title);
        if (exist)
        {
            return ResponseModel.Failure("This artwork Name already exists.");
        }

        var model = new ArtWork
        {
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            Rating = request.Rating,
            Quantity = request.Quantity,
            Price = request.Price,
            ArtImage = request.ArtImage,
            CategoryId = request.CategoryId,
            DisCount = request.DisCount,
            ProductionYear = request.ProductionYear
        };

        _dbContext.ArtWorks.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<ArtWorkModel>.Success(_mapper.Map<ArtWorkModel>(model), "ArtWork successfully created");
    }
}
