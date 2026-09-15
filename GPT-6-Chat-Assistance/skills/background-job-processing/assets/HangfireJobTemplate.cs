using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hangfire;

namespace <ProjectName>Core.Jobs
{
    public interface ISyncJob
    {
        Task ExecuteAsync(IJobCancellationToken cancellationToken);
    }

    public class SyncJob : ISyncJob
    {
        private readonly ILogger<SyncJob> _logger;
        private readonly IDataRepository _repository;

        public SyncJob(ILogger<SyncJob> logger, IDataRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ExecuteAsync(IJobCancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting background sync job.");

            // Do work, checking cancellation token if processing loops
            await _repository.SyncDataAsync();

            _logger.LogInformation("Completed background sync job.");
        }
    }
}
