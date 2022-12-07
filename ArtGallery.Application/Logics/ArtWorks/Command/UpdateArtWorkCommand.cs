using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using Microsoft.Extensions.Configuration;
using ArtGallery.Application.Common.Helpers;

namespace ArtGallery.Application.Handlers.ArtWorks.Commands;

public partial class UpdateArtWorkCommand : IRequest<string>
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

public class UpdateArtWorkCommandHandler : IRequestHandler<UpdateArtWorkCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UpdateArtWorkCommandHandler(IApplicationContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> Handle(UpdateArtWorkCommand request, CancellationToken cancellationToken)
    {
        var artWork = await _dbContext.ArtWorks.FirstOrDefaultAsync(x => x.Id == request.ArtWorkId);

        var art = _dbContext.ArtWorks;
        if (art.Any(x => x.Title == request.Title))
        {
            return "This Title already exists";
        }

        if (art.Any(x => x.Description.ToLower() == request.Description))
        {
            return "A Description same as already exist";
        }

        var ditryLan = _configuration.GetSection("DirtyLan").Get<List<string>>();
        var disAllowed = ditryLan.Contains(request.Description);
        if (disAllowed)
        {
            return "Cause words are not allowed";
        }

        var artBlackList = _configuration.GetSection("ArtBlackList").Get<List<string>>();
        if (artBlackList.Any(x => x == request.Title.ToLower()))
        {
            return "The Inputed Title isn't allowed";
        }

        var minimumPrice = _configuration.GetValue<long>("MinimumPrice");
        if (request.Price < minimumPrice)
        {
            return "The required minimum price for all artworks is 499";
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


        if (request.ArtImageUpload != null || request.ArtImageUpload?.Length > 0)
        {
            string folder = $"img/ArtImages/{artWork.Id}";
            var fileName = Guid.NewGuid().ToString();
            var filePath = Path.Combine($"wwwroot/{folder}", fileName);
            await FileHelper.UploadFile(request.ArtImageUpload, filePath);

            _dbContext.ArtWorks.Update(artWork);
            await _dbContext.SaveChangesAsync();
        }

        return "ArtWork was successfully updated";
    }
}
