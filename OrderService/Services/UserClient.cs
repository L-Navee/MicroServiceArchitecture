namespace OrderService.Services
{
    public class UserClient
    {
        private readonly HttpClient _httpClient;

        // Constructor that accepts HttpClient from DI
        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Call UserService API to validate user by ID
        public async Task<bool> ValidateUserAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5278/api/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsStringAsync();
                // Here, you could parse the user and do more logic
                return !string.IsNullOrEmpty(user);
            }
            return false;
        }
    }
}
