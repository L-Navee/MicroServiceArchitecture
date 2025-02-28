using Microsoft.AspNetCore.Mvc;
using OrderService.Controllers.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrdersService _orderService; // Use OrderService from the OrderServices namespace
        private readonly UserClient _userClient;

        // Inject OrderService and UserClient into the constructor
        public OrderController(OrdersService orderService, UserClient userClient)
        {
            _orderService = orderService;
            _userClient = userClient;
        }

        [HttpPost("create-orders")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            // Validate user before creating order
            bool isUserValid = await _userClient.ValidateUserAsync(orderRequest.UserId);

            if (!isUserValid)
            {
                return BadRequest("Invalid user.");
            }

            // Create a new Order object from OrderRequest
            var order = new Order
            {
                UserId = orderRequest.UserId,
                ProductName = orderRequest.ProductName,
                Quantity = orderRequest.Quantity,
                OrderDate = DateTime.UtcNow
            };

            // Call OrderService to save the order
            var response = await _orderService.CreateOrderAsync(order);

            if (response.Success)
            {
                return Ok(response);  // Return a success response with order details
            }

            return BadRequest(response.Message);  // Return failure message
        }
    }

    // Example of OrderRequest class
    public class OrderRequest
    {
        public string UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
