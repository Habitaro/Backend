using WebApi.Models.Services.Abstractions;

namespace WebApi.Startup
{
    public class SeedHabitProgressHostedService : IHostedService
    {
        private IServiceProvider services;

        public SeedHabitProgressHostedService(IServiceProvider services)
        {
            this.services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            using var unit = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unit.HabitService.SeedProgress(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
