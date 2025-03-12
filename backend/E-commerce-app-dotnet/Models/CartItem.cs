using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace E_commerce_app_dotnet.Models
{
    [BsonIgnoreExtraElements]
    public class CartItem
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }
        public Product Product { get; set; } 
    }
}
