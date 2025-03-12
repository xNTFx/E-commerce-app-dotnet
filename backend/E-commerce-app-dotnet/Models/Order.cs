using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace E_commerce_app_dotnet.Models
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("surname")]
        public string Surname { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("zipCode")]
        public string ZipCode { get; set; }

        [BsonElement("cityTown")]
        public string CityTown { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("products")]
        public List<ProductWithCount> Products { get; set; }

        [BsonElement("total")]
        public double Total { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("createDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "PENDING";

        public Order()
        {
            CreateDate = DateTime.Now;
        }
    }
}