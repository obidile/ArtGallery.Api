using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderModel>
    {
        public GetOrderByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderModel>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<OrderModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Orders.AsNoTracking().Where(c => c.Id == request.Id).ProjectTo<OrderModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (data == null)
            {
                throw new("No Order with the specified ID was found.");
            }

            return data;
        }
    }
}
