using MongoDB.Driver;
using UserService.Models;

namespace UserService.Services 
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _users = database.GetCollection<User>(config["MongoDB:CollectionName"]);
        }

        public async Task<List<User>> GetUsersAsync() => await _users.Find(user => true).ToListAsync();
        public async Task<User> GetUserAsync(string id) => await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        public async Task CreateUserAsync(User user) => await _users.InsertOneAsync(user);
    }
}
