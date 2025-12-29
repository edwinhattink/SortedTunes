using System.Text.Json.Serialization;
using SortedTunes.Application;
using SortedTunes.Infrastructure;
using SortedTunes.Infrastructure.Data;
using SortedTunes.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseCors("_developmentOrigins");
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await app.InitialiseDatabaseAsync();

app.UseHealthChecks("/api/Health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});
app.UseSwaggerUi(settings =>
{
    settings.Path = "/internal-api";
    settings.DocumentPath = "/api/internal-specification.json";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapEndpoints();

await app.RunAsync();

public partial class Program { }
