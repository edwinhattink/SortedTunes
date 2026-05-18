using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SortedTunes.Application.Common.Interfaces;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Infrastructure.Data;
using SortedTunes.Infrastructure.Data.Interceptors;
using SortedTunes.Infrastructure.Services.Cache;
using SortedTunes.Infrastructure.Services.Elasticsearch;
using SortedTunes.Infrastructure.Services.Elasticsearch.Config;

namespace SortedTunes.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        // Elasticsearch
        builder.Services.Configure<ElasticsearchOptions>(builder.Configuration.GetSection("Elasticsearch"));
        builder.Services.AddScoped((provider) =>
        {
            var elasticsearchOptions = provider.GetRequiredService<IOptions<ElasticsearchOptions>>().Value;
            return elasticsearchOptions.CreateClient();
        });
        builder.Services.AddScoped<IElasticsearchLoggingService, ElasticsearchLoggingService>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        builder.Services.AddSingleton(TimeProvider.System);

        // Caching
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICacheProvider, CacheProvider>();
        builder.Services.AddScoped<IMemoryWrapper, MemoryWrapper>();
    }
}
