namespace E_commerce_app_dotnet.Models
{
    public class CartItemRequest
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Count { get; set; }
    }
}
