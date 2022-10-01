using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Categories.Queries // Category
{
    public class GetCategoryByIdQuery : IRequest<CategoryModel>
    {
        public GetCategoryByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
    {
        public GetCategoryByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryModel>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryModel> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Categories.AsNoTracking().Where(c => c.Id == request.Id).ProjectTo<CategoryModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (data == null)
            {
                throw new("No Category with the specified ID was found.");
            }

            return data;
        }
    }
}