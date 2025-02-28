using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // 👈 Ensures `_id` is stored as a string instead of ObjectId
        public string Id { get; set; }

        [BsonElement("name")]  // 👈 Ensures it maps to MongoDB's `name` field
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
