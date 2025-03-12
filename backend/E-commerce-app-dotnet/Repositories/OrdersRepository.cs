using E_commerce_app_dotnet.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using E_commerce_app_dotnet.Repositories.Interfaces;

namespace E_commerce_app_dotnet.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public OrdersRepository(IConfiguration config, IMongoClient client)
        {
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _collection = database.GetCollection<Order>("orders");
        }

        public List<Order> FindByUserId(string userId)
        {
            return _collection.Find(order => order.UserId == userId).ToList();
        }

        public Order Save(Order order)
        {
            _collection.InsertOne(order);
            return order;
        }

        public Order FindById(string orderId)
        {
            return _collection.Find(order => order._id == orderId).FirstOrDefault();
        }
    }
}
