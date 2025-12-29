using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SortedTunes.Application;
using SortedTunes.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Environment.EnvironmentName = GetEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "dev");

var env = builder.Environment;
builder.Configuration.Sources.Clear();
var currentDir = Directory.GetCurrentDirectory();
var webPath = Path.GetFullPath(Path.Combine(currentDir, "src/Web"));
if (!Directory.Exists(webPath))
{
    var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    webPath = Path.GetFullPath(Path.Combine(exeDir, "../../../../Web"));
}

builder.Configuration.SetBasePath(webPath);
builder.Configuration.AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

// Add services
builder.AddApplicationServices();
builder.AddInfrastructureServices();

builder.Services.AddSingleton(TimeProvider.System);
//builder.Services.AddScoped<ICurrentUserService, ToolsUserService>();

var host = builder.Build();

return await CommandRouter(host, args);

static async Task<int> CommandRouter(IHost host, string[] args)
{
    using var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

    // Find the command from arguments
    var command = args.FirstOrDefault(a => a.StartsWith("refresh", StringComparison.OrdinalIgnoreCase))?.ToLowerInvariant();
    var resetIndex = args.Any(a => string.Equals(a, "resetIndex", StringComparison.OrdinalIgnoreCase));
    string sender = "Tools";

    return command switch
    {
        "refresh" => await ExecuteCommands(host, [
            //new CreateTestProductCommand() { Sender = sender },
            //new RefreshElasticProductsCommand() { Sender = sender, ResetIndex = resetIndex }
        ], cts.Token),
        "refresh-elastic" => await ExecuteCommands(host, [
            //new RefreshElasticProductsCommand() { Sender = sender, ResetIndex = resetIndex }
        ], cts.Token),
        "refresh-test-products" => await ExecuteCommands(host, [
            //new CreateTestProductCommand() { Sender = sender },
        ], cts.Token),
        _ => await ExecuteCommands(host, [], cts.Token),
    };
}

static async Task<int> ExecuteCommands(IHost host, ICollection<IRequest> commands, CancellationToken cancellationToken)
{
    await using var scope = host.Services.CreateAsyncScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    foreach (var command in commands)
    {
        try
        {
            await mediator.Send(command, cancellationToken);
            Console.WriteLine($"✓ {command.GetType().Name} command completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error executing command {command.GetType().Name}: {ex.Message}");
            return 1;
        }
    }
    return 0;
}

static string GetEnvironment(string environment) => environment switch
{
    "tst" => "Test",
    "acc" => "Acceptance",
    "prd" => "Production",
    _ => "Development"
};
