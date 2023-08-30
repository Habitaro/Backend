using WebApi.Models.Services.Abstractions;

namespace WebApi.Startup
{
    public class SeedHabitProgressHostedService : IHostedService
    {
        public IUnitOfWork Unit { get; set; }

        public SeedHabitProgressHostedService(IUnitOfWork unit)
        {
            Unit = unit;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Unit.HabitService.SeedProgress(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
