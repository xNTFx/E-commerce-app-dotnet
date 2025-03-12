using System.Collections.Generic;
using E_commerce_app_dotnet.Models;

namespace E_commerce_app_dotnet.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        List<Order> FindByUserId(string userId);
        Order Save(Order order);
        Order FindById(string orderId);
    }
}
