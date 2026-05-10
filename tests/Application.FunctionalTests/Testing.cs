using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using SortedTunes.Domain.Common;
using SortedTunes.Infrastructure.Data;
using SortedTunes.Mediator;
using SortedTunes.Mediator.Contracts;

namespace SortedTunes.Application.FunctionalTests;

#pragma warning disable NUnit1028

[SetUpFixture]
public partial class Testing
{
    private static ITestDatabase s_database = null!;
    private static CustomWebApplicationFactory s_factory = null!;
    private static IServiceScopeFactory s_scopeFactory = null!;
    public static FakeTimeProvider TimeProviderInstance { get; set; } = null!;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var fixture = ApplicationAutoDataAttribute.CreateFixture();

        s_database = await TestDatabaseFactory.CreateAsync();

        s_factory = new CustomWebApplicationFactory(
            services =>
            {
                TimeProviderInstance = new FakeTimeProvider();
                services.Remove<TimeProvider>().AddTransient<TimeProvider>(sp => TimeProviderInstance);
            },
            s_database.GetConnection()
        );

        s_scopeFactory = s_factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = s_scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        try
        {
            return await mediator.Send(request);
        }
        catch (ValidationException e)
        {
            throw new ValidationException($"{e.Message} Errors: {JsonSerializer.Serialize(e.Errors)}", e.Errors);
        }
    }

    public static async Task SendAsync(IRequest request)
    {
        using var scope = s_scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        try
        {
            await s_database.ResetAsync();
        }
        catch (Exception)
        {
            // Ignore exceptions during reset to avoid test failures
        }
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var entity = await context.FindAsync<TEntity>(keyValues);

        Guard.Against.Null(entity);

        return entity;
    }

    public static async Task<TEntity> FindAsync<TEntity>(
        int id,
        Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includes)
        where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Get entity metadata to identify the primary key property
        var entityType = context.Model.FindEntityType(typeof(TEntity))
            ?? throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} not found in model.");

        var keyProperty = entityType.FindPrimaryKey()?.Properties[0]
            ?? throw new InvalidOperationException($"No primary key defined for {typeof(TEntity).Name}.");

        var keyName = keyProperty.Name;

        // Start query and apply includes if provided
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = includes.Compile().Invoke(query);

        // Build dynamic lambda: e => e.Id == id
        var parameter = Expression.Parameter(typeof(TEntity), "e");
        var propertyAccess = Expression.Property(parameter, keyName);
        var idValue = Expression.Constant(id);
        var equalExpression = Expression.Equal(propertyAccess, idValue);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);

        var entity = await query.FirstOrDefaultAsync(lambda);

        Guard.Against.Null(entity);

        return entity;
    }

    public static async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().Where(expression).ToListAsync();
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task HandleDomainEvents(List<BaseEvent> domainEvents)
    {
        using var scope = s_scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }

    public static async Task<int> AddRangeAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.AddRange(entities);

        await context.SaveChangesAsync();

        return entities.Count();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public static async Task UpdateAsync<TEntity>(TEntity entity)
      where TEntity : class
    {
        using var scope = s_scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Update(entity);

        await context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await s_database.DisposeAsync();
        await s_factory.DisposeAsync();
    }
}
