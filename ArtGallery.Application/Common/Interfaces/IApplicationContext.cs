using ArtGallery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Common.Interfaces;

public interface IApplicationContext
{
    DbSet<User> Users { get; set; }
    DbSet<ArtWork> ArtWorks { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<Cart> Carts { get; set; }
    DbSet<CartItem> CartItems { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<Order> Orders { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
