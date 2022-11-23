using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.Orders.Command;
using ArtGallery.Application.Logics.Users;
using ArtGallery.Domain.Entities;
using AutoMapper;

namespace ArtGallery.Application.Common.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Cart, CartModel>().ReverseMap();
            CreateMap<ArtWork, ArtWorkModel>().ReverseMap();
            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<OrderItem, OrderItemModel>().ReverseMap();
        }
    }
}
