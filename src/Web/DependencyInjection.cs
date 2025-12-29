using System.Security.Claims;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;
using SortedTunes.Infrastructure.Data;
using SortedTunes.Infrastructure.Services.Elasticsearch;
using SortedTunes.Web.Filters;
using ZymLabs.NSwag.FluentValidation;


namespace SortedTunes.Web;

public static class DependencyInjection
{
    const string Audience = "buynamics-wtp-cost-models-api";

    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        //builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks()
            .AddCheck<ElasticsearchHealthService>("elasticsearch")
            .AddDbContextCheck<ApplicationDbContext>("sqldatabase");

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options => options.AddPolicy(
                name: "_developmentOrigins",
                policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));
        }

        // Auth0
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration.GetValue<string>("Auth0:Domain");
                options.Audience = Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        builder.Services.AddRazorPages();

        builder.Services.AddScoped(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApiDocument((configure, serviceProvider) =>
        {
            configure.Title = "SortedTunes API";
            configure.OperationProcessors.Add(new SwaggerPublicEndpointFilter());

            ConfigureOpenApiDocument(configure, serviceProvider);
        });

        builder.Services.AddOpenApiDocument((configure, serviceProvider) =>
        {
            configure.DocumentName = "internal";
            configure.Title = "SortedTunes Internal API";

            ConfigureOpenApiDocument(configure, serviceProvider);
        });
    }

    public static void AddKeyVaultIfConfigured(this IHostApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }
    }

    private static void ConfigureOpenApiDocument(AspNetCoreOpenApiDocumentGeneratorSettings configure, IServiceProvider serviceProvider)
    {
        var fluentValidationSchemaProcessor = serviceProvider.CreateScope()
                .ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

        // Add the fluent validations schema processor
        configure.SchemaSettings.SchemaProcessors.Add(fluentValidationSchemaProcessor);

        configure.SchemaSettings.TypeMappers.Add(new PrimitiveTypeMapper(
            typeof(ValueTuple<int, int>),
            schema =>
            {
                schema.Type = JsonObjectType.String;
                schema.Pattern = @"^\d{4}-(0[1-9]|1[0-2])$";
                schema.Example = "2025-11";
                schema.Description = "Month in YYYY-MM format";
            })
        );

        configure.AddSecurity("JWT", [], new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "Type into the textbox: Bearer {your JWT token}."
        });

        configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
    }
}
