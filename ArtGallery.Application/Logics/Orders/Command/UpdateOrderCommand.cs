using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Enums;

namespace ArtGallery.Application.Handlers.Orders.Commands;

public partial class UpdateOrderCommand : IRequest<ResponseModel>
{
    public long OrderId { get; set; }
    public OtherStatus Status { get; set; }
    public long OrderAmount { get; set; }
    public long AmountPaid { get; set; }
    public bool PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(v => v.OrderId).NotEmpty();
    }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId);

        if (order == null)
        {
            return ResponseModel.Failure("Order not found");
        }

        order.Status = request.Status;
        order.OrderAmount = request.OrderAmount;
        order.AmountPaid = request.AmountPaid;
        order.PaymentStatus = request.PaymentStatus;
        order.PaymentDate = request.PaymentDate;
        order.UpdatedDate = request.UpdatedDate;


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<OrderModel>.Success(_mapper.Map<OrderModel>(order), "Order was successfully updated");
    }
}
