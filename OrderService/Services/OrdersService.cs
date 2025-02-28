using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Controllers.Models;

namespace OrderService.Services
{
    public class OrdersService
    {
            private readonly IMongoCollection<Order> _orders;

            public OrdersService(IConfiguration config)
            {
                var client = new MongoClient(config.GetValue<string>("MongoDbConnection"));
                var database = client.GetDatabase("OrderDatabase");
                _orders = database.GetCollection<Order>("Orders");
            }

            // Method to create a new order
            public async Task<CreateOrderResponse> CreateOrderAsync(Order order)
            {
                try
                {
                    order.OrderDate = DateTime.UtcNow;

                    // Insert order into MongoDB
                    await _orders.InsertOneAsync(order);

                    return new CreateOrderResponse
                    {
                        Success = true,
                        Message = "Order created successfully!",
                        OrderId = order.Id.ToString()
                    };
                }
                catch (Exception ex)
                {
                    return new CreateOrderResponse
                    {
                        Success = false,
                        Message = $"Failed to create order: {ex.Message}"
                    };
                }
            }

            // Method to get order details by order ID
            public async Task<Order> GetOrderAsync(string orderId)
            {
                try
                {
                    // Use ObjectId to query MongoDB by orderId (assuming it's stored as ObjectId in MongoDB)
                    var order = await _orders.Find(o => o.Id == ObjectId.Parse(orderId)).FirstOrDefaultAsync();
                    return order;
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Invalid OrderId format.");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to fetch order: {ex.Message}");
                }
            }

            // Method to get all orders for a user
            public async Task<List<Order>> GetOrdersByUserAsync(string userId)
            {
                try
                {
                    return await _orders.Find(o => o.UserId == userId).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to fetch orders for user: {ex.Message}");
                }
            }

            // Method to update an order's details
            public async Task<bool> UpdateOrderAsync(string orderId, Order updatedOrder)
            {
                try
                {
                    var result = await _orders.ReplaceOneAsync(
                        o => o.Id == ObjectId.Parse(orderId), updatedOrder);

                    return result.IsAcknowledged && result.ModifiedCount > 0;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to update order: {ex.Message}");
                }
            }

            // Method to delete an order by ID
            public async Task<bool> DeleteOrderAsync(string orderId)
            {
                try
                {
                    var result = await _orders.DeleteOneAsync(o => o.Id == ObjectId.Parse(orderId));

                    return result.IsAcknowledged && result.DeletedCount > 0;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to delete order: {ex.Message}");
                }
            }
        }

        // CreateOrderResponse model to return a response from CreateOrderAsync
        public class CreateOrderResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string OrderId { get; set; }
        }
    }


