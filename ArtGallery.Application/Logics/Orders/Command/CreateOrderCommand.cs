using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using ArtGallery.Domain.Enums;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Orders.Command;

public class CreateOrderCommand : IRequest<string>
{
    public OtherStatus Status { get; set; }
    public long OrderAmount { get; set; }
    public long AmountPaid { get; set; }
    public bool PaymentStatus { get; set; }
    public DateTime orderDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateOrderCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<Order>(request);

        _dbContext.Orders.Add(model);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return "Your Order has been placed";
    }
}
