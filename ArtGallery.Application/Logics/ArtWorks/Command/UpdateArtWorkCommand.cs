using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using Microsoft.AspNetCore.Http;
using ArtGallery.Application.Common.Helpers;
using Serilog;

namespace ArtGallery.Application.Handlers.ArtWorks.Commands;

public partial class UpdateArtWorkCommand : IRequest<ResponseModel>
{
    public long ArtWorkId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long Rating { get; set; }
    public long Quantity { get; set; }
    public long Price { get; set; }
    public string Category { get; set; }
    public IFormFile ArtImageUpload { get; set; }
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
        artWork.UpdateDate = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);


        if (request.ArtImageUpload == null || request.ArtImageUpload.Length == 0)
        {
            try
            {
                var filePath = $"wwwroot/img/Artworks/{artWork.Id}";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var filename = Guid.NewGuid().ToString() + "_" + request.ArtImageUpload.FileName;
                filePath = Path.Combine(filePath, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    request.ArtImageUpload.CopyTo(fileStream);
                }

                artWork.ArtImage = filePath.Replace("wwwroot/", "");
                _dbContext.ArtWorks.Update(artWork);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Artwork Upload");
                return ResponseModel.Failure("Your request was saved but error occure while processing your image.");
            }
        }

        return ResponseModel<ArtWorkModel>.Success(_mapper.Map<ArtWorkModel>(artWork), "ArtWork was successfully updated");
    }
}
