using Microsoft.EntityFrameworkCore;

namespace BlazorApp
{
    public class StartUpService(IDbContextFactory<AppDbContext> contextFactory) : IHostedService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}