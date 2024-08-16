using App.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Core.Extensions;

public static class WebApplicationExtensions
{
    public static void AddRemoveResponseHeaders(this WebApplication app)
    {
        // Add additional headers
        app.Use(async (context, next) =>
        {
            // Block other website capability to put our site into iframe
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("x-xss-protection", "1; mode=block");
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("Server");

            await next();
        });
    }

    public static void RunPendingMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            ILogger<ApplicationDbContext> logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

            try
            {
                #region Run Pending Migrations
                try
                {
                    if (context.Database.GetPendingMigrations().Any()) //Check for pending migrations
                    {
                        context.Database.Migrate(); // run migration (update-database)
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(exception: ex, $"Run pending migrations failed [{DateTime.UtcNow}]");
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, $"Run pending migrations failed [{DateTime.UtcNow}]");
            }
        }
    }
}
