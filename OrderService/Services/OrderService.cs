using Grpc.Core;
using OrderService.Data;
using OrderModel = OrderService.Models.Order;

namespace OrderService.Services
{
    public class OrderService : Order.OrderBase
    {
        private readonly OrderContext _context;

        public OrderService(OrderContext context)
        {
            _context = context;
        }

        public override async Task<OrderReply> PlaceOrder(OrderRequest request, ServerCallContext context)
        {
            var order = new OrderModel
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UserId = request.UserId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderReply
            {
                Id = order.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                UserId = order.UserId
            };
        }

        public override async Task<OrderReply> GetOrder(OrderQuery query, ServerCallContext context)
        {
            OrderModel order = await _context.Orders.FindAsync(query.OrderId);
            return order == null
                ? throw new RpcException(new Status(StatusCode.NotFound, "Order not found"))
                : new OrderReply
                {
                    Id = order.Id,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                    UserId = order.UserId
                };
        }

        public override async Task<OrdersReply> GetOrdersByUser(UserQuery request, ServerCallContext context)
        {
            var orders = _context.Orders.Where(o => o.UserId == request.UserId).ToList();

            var orderReplies = orders.Select(o => new OrderReply
            {
                Id = o.Id,
                ProductId = o.ProductId,
                Quantity = o.Quantity,
                UserId = o.UserId
            }).ToList();

            return new OrdersReply
            {
                Orders = { orderReplies }
            };
        }
    }
}
