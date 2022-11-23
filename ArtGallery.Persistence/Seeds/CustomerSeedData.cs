using ArtGallery.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ArtGallery.Persistence.Seeds;

public class CustomerSeedData : IRequest<string>
{
    public class CustomerSeedDataCommandHandler : IRequestHandler<CustomerSeedData, string>
    {
        private readonly IApplicationContext _dbContext;
        private readonly IMapper _mapper;
        public CustomerSeedDataCommandHandler(IApplicationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> Handle(CustomerSeedData request, CancellationToken cancellationToken)
        {
            _dbContext.Customers.AddRange(
                new Domain.Entities.Customer
                {
                    firstName = "sammy",
                    lastName = "lammy",
                    mailAddress = "lammy@gmail.com",
                    dateOfBirth = DateTime.Parse("1998-2-12"),
                    CreatedDate = DateTime.Now,
                    CreatedBy = "Seed Test"
                },
                 new Domain.Entities.Customer
                 {
                     firstName = "jammy",
                     lastName = "demmy",
                     mailAddress = "demmy@gmail.com",
                     dateOfBirth = DateTime.Parse("1999-2-12"),
                     CreatedDate = DateTime.Now,
                     CreatedBy = "Seed Test"
                 },
                  new Domain.Entities.Customer
                  {
                      firstName = "emmy",
                      lastName = "jommy",
                      mailAddress = "jommy@gmail.com",
                      dateOfBirth = DateTime.Parse("1998-2-12"),
                      CreatedDate = DateTime.Now,
                      CreatedBy = "Seed Test"
                  }
                );
           await _dbContext.SaveChangesAsync();
            return "Seed Data added";
        }
    }
}
