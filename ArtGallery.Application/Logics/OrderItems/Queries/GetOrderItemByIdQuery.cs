using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.OrderItems.Queries
{
    public class GetOrderItemByIdQuery : IRequest<OrderItemModel>
    {
        public GetOrderItemByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class GetOrderItemByIdQueryValidator : AbstractValidator<GetOrderItemByIdQuery>
    {
        public GetOrderItemByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, OrderItemModel>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderItemByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<OrderItemModel> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.OrderItems.AsNoTracking().Where(c => c.Id == request.Id).ProjectTo<OrderItemModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (data == null)
            {
                throw new("No Order item with the specified ID was found.");
            }

            return data;
        }
    }
}
