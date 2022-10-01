using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserModel>
    {
        public GetUserByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("Id is required.");
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserModel>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.Users.AsNoTracking().Where(c => c.Id == request.Id).ProjectTo<UserModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (data == null)
            {
                throw new ("No User with the specified ID was found.");
            }

            return data;
        }
    }
}
