using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace E_commerce_app_dotnet
{
    public static class ApplicationConfiguration
    {
        public static WebApplication ConfigureApplication(this WebApplication app)
        {
            app.Logger.LogInformation("Inicjalizacja aplikacji...");

            app.UseRouting();

            app.UseCors("AllowAll");

            app.Use(async (context, next) =>
            {
                app.Logger.LogInformation("Otrzymano żądanie: {Method} {Path}", context.Request.Method, context.Request.Path);
                if (context.Request.Headers.ContainsKey("Origin"))
                {
                    app.Logger.LogInformation("Żądanie pochodzi z: {Origin}", context.Request.Headers["Origin"]);
                }
                await next();
            });

            app.UseAuthorization();
            app.MapControllers();

            app.Use(async (context, next) =>
            {
                await next();
                app.Logger.LogInformation("Nagłówki odpowiedzi: {Headers}", context.Response.Headers);
            });

            return app;
        }
    }
}