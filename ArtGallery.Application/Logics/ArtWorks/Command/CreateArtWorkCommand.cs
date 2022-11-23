using ArtGallery.Application.Common.Helpers;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ArtGallery.Application.Logics.ArtWorks.Command;

public class CreateArtWorkCommand : IRequest<string>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public long Rating { get; set; }
    public long Quantity { get; set; }
    public long Price { get; set; }
    public IFormFile ArtImageUpload { get; set; }
    public long? DisCount { get; set; }
    public DateTime ProductionYear { get; set; }
}
public class CreateArtWorkCommandHandler : IRequestHandler<CreateArtWorkCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public CreateArtWorkCommandHandler(IApplicationContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    public async Task<string> Handle(CreateArtWorkCommand request, CancellationToken cancellationToken)
    {
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


        var model = new ArtWork
        {
            Title = request.Title,
            Description = request.Description,
            Rating = request.Rating,
            Quantity = request.Quantity,
            Price = request.Price,
            DisCount = request.DisCount,
            ProductionYear = request.ProductionYear,
            CreatedDate = DateTime.Now,
        };

        _dbContext.ArtWorks.Add(model);
        await _dbContext.SaveChangesAsync();

        if (request.ArtImageUpload != null || request.ArtImageUpload?.Length > 0)
        {
            string folder = $"img/ArtImages/{model.Id}";
            var fileName = Guid.NewGuid().ToString();
            var filePath = Path.Combine($"wwwroot/{folder}", fileName);
            await FileHelper.UploadFile(request.ArtImageUpload, filePath);

            _dbContext.ArtWorks.Update(model);
            await _dbContext.SaveChangesAsync();
        }

        return  "ArtWork successfully created";
    }
}
