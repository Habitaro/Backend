using WebApi.Models.Services.Abstractions;

namespace WebApi.Startup
{
    public class SeedHabitProgressHostedService : IHostedService
    {
        private readonly IServiceProvider services;

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

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => Console.WriteLine("SeedHabitProgressHostedService is shut down"), cancellationToken);
        }
    }
}
