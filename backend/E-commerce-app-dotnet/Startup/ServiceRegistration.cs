using E_commerce_app_dotnet.Repositories;
using E_commerce_app_dotnet.Repositories.Interfaces;
using E_commerce_app_dotnet.Services;
using E_commerce_app_dotnet.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace E_commerce_app_dotnet
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("https://e-commerce-app-dotnet.pawelsobon.pl", "https://www.e-commerce-app-dotnet.pawelsobon.pl")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddSingleton<IMongoClient>(new MongoClient(config["MongoDB:Uri"]));

            services.AddSingleton<IProductsRepository, ProductsRepository>();
            services.AddSingleton<ICategoriesRepository, CategoriesRepository>();
            services.AddSingleton<ICartItemsRepository, CartItemsRepository>();
            services.AddSingleton<IOrdersRepository, OrdersRepository>();

            services.AddSingleton<IProductsService, ProductsService>();
            services.AddSingleton<ICategoriesService, CategoriesService>();
            services.AddSingleton<ICartItemsService, CartItemsService>();
            services.AddSingleton<IOrdersService, OrdersService>();
            services.AddSingleton<IFirebaseAuthService, FirebaseAuthService>();
            services.AddSingleton<IStripeService, StripeService>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
