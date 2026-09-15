using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace <ProjectName>Core.Service
{
    public class ExampleLoggingService
    {
        private readonly ILogger<ExampleLoggingService> _logger;

        public ExampleLoggingService(ILogger<ExampleLoggingService> logger)
        {
            _logger = logger;
        }

        public async Task ProcessDataAsync(int userId)
        {
            _logger.LogInformation("Starting data processing for user {UserId}", userId);

            // Business logic here...

            _logger.LogInformation("Successfully completed data processing for user {UserId}", userId);
        }
    }
}
