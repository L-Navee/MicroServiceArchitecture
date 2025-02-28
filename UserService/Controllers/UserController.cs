using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService.Services.UserService _userService;
        private readonly OrderClient _orderClient;  // Add OrderClient

        public UserController(UserService.Services.UserService userService, OrderClient orderClient)
        {
            _userService = userService;
            _orderClient = orderClient;  // Initialize OrderClient
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetUsers() => Ok(await _userService.GetUsersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // New endpoint to fetch order details from OrderService
        [HttpGet("{id}/orders/{orderId}")]
        public async Task<IActionResult> GetUserOrderDetails(string id, string orderId)
        {
            var orderDetails = await _orderClient.GetOrderDetailsAsync(orderId);
            if (orderDetails == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(orderDetails);
        }
    }
}
