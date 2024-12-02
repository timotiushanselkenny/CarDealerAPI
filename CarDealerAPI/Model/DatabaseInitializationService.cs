using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarDealerAPI.Model
{
    public class DatabaseInitializationService : IHostedService
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseInitializationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Call Init when the application starts
            await _databaseContext.Init();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // You can perform any cleanup here, if needed
            return Task.CompletedTask;
        }
    }
}
