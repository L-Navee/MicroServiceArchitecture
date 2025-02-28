using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderService.Controllers.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string UserId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
