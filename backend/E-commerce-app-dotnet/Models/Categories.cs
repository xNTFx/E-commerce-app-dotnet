using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace E_commerce_app_dotnet.Models
{
    [BsonIgnoreExtraElements]
    public class Categories
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;
    }
}
