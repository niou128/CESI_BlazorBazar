using Microsoft.EntityFrameworkCore;
using OrderModel = OrderService.Models.Order;

namespace OrderService.Data
{
    public class OrderContext : DbContext
    {
        public DbSet<OrderModel> Orders { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }
    }
}
