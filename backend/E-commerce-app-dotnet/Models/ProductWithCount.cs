using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace E_commerce_app_dotnet.Models
{
    [BsonIgnoreExtraElements]
    public class ProductWithCount
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("discountPercentage")]
        public double DiscountPercentage { get; set; }

        [BsonElement("rating")]
        public double Rating { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("brand")]
        public string Brand { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("thumbnail")]
        public string Thumbnail { get; set; }

        [BsonElement("images")]
        public List<string> Images { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("totalPrice")]
        public double TotalPrice { get; set; }
    }
}
