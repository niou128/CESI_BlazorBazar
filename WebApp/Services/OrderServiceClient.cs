using Grpc.Core;
using Grpc.Net.Client;
using OrderService;

namespace WebApp.Services
{
    public class OrderServiceClient
    {
        private readonly Order.OrderClient _client;

        public OrderServiceClient(GrpcChannel channel)
        {
            _client = new Order.OrderClient(channel);
        }

        public async Task<List<OrderReply>> GetOrdersForUser(string userId)
        {
            var request = new UserQuery { UserId = userId };
            try
            {
                var reply = await _client.GetOrdersByUserAsync(request);
                return new List<OrderReply>(reply.Orders);
            }
            catch (RpcException e)
            {
                Console.WriteLine($"gRPC call error: {e.Status.Detail}");
                return new List<OrderReply>();
            }
        }

        public async Task<OrderReply> PlaceOrderAsync(OrderRequest request)
        {
            try
            {
                return await _client.PlaceOrderAsync(request);
            }
            catch (RpcException e)
            {
                Console.WriteLine($"gRPC call error: {e.Status.Detail}");
                throw;
            }
        }
    }
}
