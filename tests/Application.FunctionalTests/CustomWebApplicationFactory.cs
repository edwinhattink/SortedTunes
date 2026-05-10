using System.Data.Common;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SortedTunes.Infrastructure.Data;
using SortedTunes.Mediator;

namespace SortedTunes.Application.FunctionalTests;

public class CustomWebApplicationFactory(
    Action<IServiceCollection> serviceCollection,
    DbConnection connection
) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //services.RemoveAll<ICurrentUserService>()
            //    .AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddMediator(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseSqlServer(connection);
                });
        });

        builder.ConfigureServices(serviceCollection.Invoke);
    }
}
