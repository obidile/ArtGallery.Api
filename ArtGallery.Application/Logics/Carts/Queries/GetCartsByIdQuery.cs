using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Carts.Queries
{
    public class GetCartByIdQuery : IRequest<CartModel>
    {
        public GetCartByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class GetCartByIdQueryValidator : AbstractValidator<GetCartByIdQuery>
    {
        public GetCartByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, CartModel>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public GetCartByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CartModel> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Carts.AsNoTracking().Where(c => c.Id == request.Id).ProjectTo<CartModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (data == null)
            {
                throw new("No Cart with the specified ID was found.");
            }

            return data;
        }
    }
}
