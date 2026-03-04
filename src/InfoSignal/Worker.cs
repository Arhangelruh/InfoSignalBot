using InfoSignal.Models;
using Microsoft.Extensions.Options;

namespace InfoSignal
{
	public class Worker(ILogger<Worker> logger, IOptions<TimeSettings> options) : BackgroundService
	{
		private readonly ILogger<Worker> _logger = logger;
		private readonly TimeSettings _options = options.Value;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var start = TimeSpan.Parse(_options.StartTime);
					var end = TimeSpan.Parse(_options.EndTime);
					var interval = int.Parse(_options.TimeInterval);
					var now = DateTime.Now.TimeOfDay;
					if (now >= start && now <= end)
					{
						//Do something						
					}
				}
				catch (Exception ex) {
					_logger.LogError(ex.Message);
					await Task.Delay(1000, stoppingToken);
				}								
			}
		}
	}
}
