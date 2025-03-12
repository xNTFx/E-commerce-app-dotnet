using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace E_commerce_app_dotnet
{
    public static class ApplicationConfiguration
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            app.UseRouting();

            app.UseCors("AllowSpecificOrigins");
            
            app.Use(async (context, next) =>
            {
                await next();
            });

            app.UseAuthorization();
            app.MapControllers();

            app.Use(async (context, next) =>
            {
                await next();
            });

            return app;
        }
    }
}
