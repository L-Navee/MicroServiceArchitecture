using System.Net.Http;
using System.Threading.Tasks;

namespace UserService.Services
{
    public class OrderClient
    {
        private readonly HttpClient _httpClient;

        public OrderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Call OrderService API to get order details by OrderId
        public async Task<string> GetOrderDetailsAsync(string orderId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5011/api/order/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                var orderDetails = await response.Content.ReadAsStringAsync();
                return orderDetails;
            }
            return null;
        }
    }
}
