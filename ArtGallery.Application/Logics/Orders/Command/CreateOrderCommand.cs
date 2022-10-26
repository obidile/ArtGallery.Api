using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using ArtGallery.Domain.Enums;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Orders.Command;

public class CreateOrderCommand : IRequest<ResponseModel>
{
    public long OrderId { get; set; }
    public OtherStatus Status { get; set; }
    public long OrderAmount { get; set; }
    public long AmountPaid { get; set; }
    public bool PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).Empty();
        RuleFor(v => v.Status).NotEmpty();
        RuleFor(v => v.OrderAmount);
        RuleFor(x => x.AmountPaid);
        RuleFor(x => x.PaymentStatus).Empty();
        RuleFor(x => x.PaymentDate).Empty();
        RuleFor(x => x.UpdatedDate).Empty();
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateOrderCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.Orders.AsNoTracking().AnyAsync(x => x.OrderId == request.OrderId);
        if (exist)
        {
            return ResponseModel.Failure("This artwork Name already exists.");
        }

        var model = new Order
        {
            OrderId = request.OrderId,
            Status = request.Status,
            OrderAmount = request.OrderAmount,
            AmountPaid = request.AmountPaid,
            PaymentStatus = request.PaymentStatus,
            PaymentDate = request.PaymentDate,
            UpdatedDate = request.UpdatedDate
        };

        _dbContext.Orders.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<OrderModel>.Success(_mapper.Map<OrderModel>(model), "ArtWork successfully created");
    }
}
