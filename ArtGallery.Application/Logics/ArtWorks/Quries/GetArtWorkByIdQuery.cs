using ArtGallery.Application.Common.Exceptions;
using ArtGallery.Application.Common.Helpers;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.ArtWorks.Queries
{
    public class GetArtWorkByIdQuery : IRequest<ArtWorkModel>
    {
        public GetArtWorkByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class GetArtWorkByIdQueryValidator : AbstractValidator<GetArtWorkByIdQuery>
    {
        public GetArtWorkByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetArtWorkByIdQueryHandler : IRequestHandler<GetArtWorkByIdQuery, ArtWorkModel>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public GetArtWorkByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ArtWorkModel> Handle(GetArtWorkByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.ArtWorks.AsNoTracking().Where(c => c.Id == request.Id).ProjectTo<ArtWorkModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (data == null)
            {
                ResponseModel.Failure("No Artwork with the specified ID was found.");
            }
            return data;
        }
    }
}