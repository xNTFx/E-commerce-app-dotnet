using E_commerce_app_dotnet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using E_commerce_app_dotnet.Services.Interfaces;
using E_commerce_app_dotnet.Repositories.Interfaces;

namespace E_commerce_app_dotnet.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ICartItemsService _cartItemsService;
        private readonly IProductsService _productsService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(
            IOrdersRepository ordersRepository,
            ICartItemsService cartItemsService,
            IProductsService productsService,
            IFirebaseAuthService firebaseAuthService,
            ILogger<OrdersService> logger)
        {
            _ordersRepository = ordersRepository;
            _cartItemsService = cartItemsService;
            _productsService = productsService;
            _firebaseAuthService = firebaseAuthService;
            _logger = logger;
        }

        public IFirebaseAuthService GetFirebaseAuthService()
        {
            return _firebaseAuthService;
        }

        public List<Order> GetOrders(string userId)
        {
            return _ordersRepository.FindByUserId(userId);
        }

        public Order GetOrderById(string orderId)
        {
            return _ordersRepository.FindById(orderId);
        }

        public Order CreateOrder(string userId, Order orderData)
        {
            if (orderData.Products == null || !orderData.Products.Any())
            {
                throw new InvalidOperationException("Order must contain at least one product.");
            }

            int totalQuantity = orderData.Products.Sum(pwc => pwc.Count);
            double totalPrice = orderData.Products.Sum(pwc => pwc.TotalPrice);

            orderData.UserId = userId;
            orderData.Quantity = totalQuantity;
            orderData.Total = totalPrice;
            orderData.CreateDate = DateTime.Now;

            var savedOrder = _ordersRepository.Save(orderData);
            if (!string.Equals(userId, "anonymous", StringComparison.OrdinalIgnoreCase))
            {
                _cartItemsService.DeleteAllCartItems(userId);
            }

            return savedOrder;
        }

        public Order ProcessOrder(string userId, Dictionary<string, object> requestBody)
        {
            _logger.LogInformation("received request body: {@RequestBody}", requestBody);

            if (!requestBody.ContainsKey("state"))
            {
                throw new ArgumentException("request body must contain 'state'");
            }

            object stateObject = requestBody["state"];
            _logger.LogInformation("raw state object type: {StateType}, value: {StateObject}",
                stateObject?.GetType(), JsonConvert.SerializeObject(stateObject));

            Dictionary<string, object> stateMap = null;

            try
            {
                if (stateObject is JsonElement jsonElement)
                {
                    _logger.LogInformation("state is jsonElement, deserializing...");
                    stateMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonElement.GetRawText());
                }
                else if (stateObject is Dictionary<string, object> directMap)
                {
                    _logger.LogInformation("state is already a dictionary");
                    stateMap = directMap;
                }
                else if (stateObject is JObject jObject)
                {
                    _logger.LogInformation("state is JObject, converting...");
                    stateMap = jObject.ToObject<Dictionary<string, object>>();
                }
                else if (stateObject is string stateString)
                {
                    _logger.LogInformation("state received as string: {StateString}", stateString);
                    stateMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(stateString);
                }
                else
                {
                    _logger.LogError("unexpected state format: {StateObject}", stateObject);
                    throw new ArgumentException("state should be a map or valid json");
                }

                if (stateMap.ContainsKey("state"))
                {
                    if (stateMap["state"] is Dictionary<string, object> nestedStateDict)
                    {
                        _logger.LogInformation("detected nested state dictionary, extracting...");
                        stateMap = nestedStateDict;
                    }
                    else if (stateMap["state"] is JObject nestedJObject)
                    {
                        _logger.LogInformation("detected nested JObject state, converting to dictionary...");
                        stateMap = nestedJObject.ToObject<Dictionary<string, object>>();
                    }
                    else if (stateMap["state"] is JsonElement nestedJsonElement)
                    {
                        _logger.LogInformation("detected nested JsonElement state, deserializing...");
                        stateMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(nestedJsonElement.GetRawText());
                    }
                    else
                    {
                        _logger.LogError("unsupported nested state type: {NestedStateType}", stateMap["state"]?.GetType());
                        throw new ArgumentException("invalid nested state format");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error parsing state: {StateObject}", stateObject);
                throw new ArgumentException("invalid state format", ex);
            }

            if (stateMap == null)
            {
                throw new ArgumentException("state is null or empty");
            }

            _logger.LogInformation("parsed state successfully: {@StateMap}", stateMap);

            if (!stateMap.ContainsKey("name") || !stateMap.ContainsKey("surname"))
            {
                throw new ArgumentException("user details are missing in the state");
            }

            var personJson = JsonConvert.SerializeObject(stateMap);
            var userDetails = JsonConvert.DeserializeObject<Person>(personJson);

            List<ProductWithCount> validProducts = new List<ProductWithCount>();

            if (!string.IsNullOrEmpty(userId) && !userId.Equals("anonymous", StringComparison.OrdinalIgnoreCase))
            {

                var cartItems = _cartItemsService.GetCartItems(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    throw new ArgumentException("cart is empty");
                }

                var productIds = cartItems.Select(ci => ci.ProductId).ToList();
                var products = _productsService.GetProductsByIds(productIds)
                    .ToDictionary(p => p._id, p => p);

                validProducts = cartItems
                    .Select(ci =>
                    {
                        if (!products.ContainsKey(ci.ProductId)) return null;

                        var product = products[ci.ProductId];
                        return new ProductWithCount
                        {
                            _id = product._id,
                            Title = product.Title,
                            Description = product.Description,
                            Price = product.Price,
                            DiscountPercentage = product.DiscountPercentage,
                            Count = ci.Count,
                            TotalPrice = product.Price * ci.Count,
                            Category = product.Category,
                            Brand = product.Brand,
                            Images = product.Images,
                            Rating = product.Rating,
                            Thumbnail = product.Thumbnail,
                            Stock = product.Stock,
                        };
                    })
                    .Where(p => p != null)
                    .ToList();
            }
            else if (stateMap.TryGetValue("products", out object productsObj))
            {
                var productsArray = JArray.Parse(JsonConvert.SerializeObject(productsObj));

                foreach (var productToken in productsArray)
                {
                    var productItem = productToken.ToObject<Dictionary<string, object>>();

                    string productId = productItem.ContainsKey("productId") ? productItem["productId"].ToString() : null;
                    int count = productItem.ContainsKey("count") ? Convert.ToInt32(productItem["count"]) : 1;

                    if (!string.IsNullOrEmpty(productId))
                    {
                        var product = _productsService.GetProductById(productId);
                        if (product != null)
                        {
                            validProducts.Add(new ProductWithCount
                            {
                                _id = product._id,
                                Title = product.Title,
                                Description = product.Description,
                                Price = product.Price,
                                DiscountPercentage = product.DiscountPercentage,
                                Rating = product.Rating,
                                Stock = product.Stock,
                                Category = product.Category,
                                Images = product.Images,
                                Thumbnail = product.Thumbnail,
                                Count = count,
                                TotalPrice = product.Price * count,
                                Brand = product.Brand
                            });
                        }
                    }
                }
            }

            if (validProducts.Count == 0)
            {
                throw new ArgumentException("no products in the order");
            }

            int totalQuantity = validProducts.Sum(p => p.Count);
            double totalPrice = validProducts.Sum(p => p.TotalPrice);

            var orderData = new Order
            {
                UserId = userId,
                Name = userDetails.Name,
                Surname = userDetails.Surname,
                Address = userDetails.Address,
                ZipCode = userDetails.ZipCode,
                CityTown = userDetails.CityTown,
                Phone = userDetails.Phone,
                Email = userDetails.Email,
                Products = validProducts,
                Total = totalPrice,
                Quantity = totalQuantity,
                CreateDate = DateTime.Now
            };

            var savedOrder = _ordersRepository.Save(orderData);

            if (!string.IsNullOrEmpty(userId) && !userId.Equals("anonymous", StringComparison.OrdinalIgnoreCase))
            {
                _cartItemsService.DeleteAllCartItems(userId);
            }

            return savedOrder;
        }


        public Order GetEnrichedOrder(string orderId)
        {
            var order = GetOrderById(orderId);
            if (order == null)
                return null;

            var productIds = order.Products.Select(p => p._id).Distinct().ToList();
            if (!productIds.Any())
            {
                throw new ArgumentException("Invalid order: products missing product id");
            }

            var products = _productsService.GetProductsByIds(productIds);
            if (products == null || !products.Any())
            {
                throw new ArgumentException("Invalid order: products not found");
            }

            var productMap = products.ToDictionary(p => p._id, p => p);
            order.Products = order.Products.Select(pwc =>
            {
                if (productMap.ContainsKey(pwc._id))
                {
                    var prod = productMap[pwc._id];
                    pwc.Title = prod.Title;
                    pwc.Description = prod.Description;
                    pwc.Price = prod.Price;
                    pwc.Category = prod.Category;
                    pwc.Brand = prod.Brand;
                }
                return pwc;
            }).ToList();

            return order;
        }
    }
}
