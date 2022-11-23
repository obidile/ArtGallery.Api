using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Common.Interfaces;

public class ApplicationContext : DbContext, IApplicationContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<ArtWork> ArtWorks { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }

}