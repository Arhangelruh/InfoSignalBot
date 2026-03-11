using InfoSignal.Interfaces;
using InfoSignal.Models;
using Microsoft.Extensions.Options;

namespace InfoSignal.Workers
{
	public class Worker(
		ILogger<Worker> logger,
		IOptions<TimeSettings> options,
		IProcessorService processorService) : BackgroundService
	{
		private readonly ILogger<Worker> _logger = logger;
		private readonly TimeSettings _options = options.Value;
		private readonly IProcessorService _processorService = processorService ?? throw new ArgumentNullException(nameof(processorService));

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
						await _processorService.Process(stoppingToken);
					}
					await Task.Delay(TimeSpan.FromMinutes(interval), stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
					await Task.Delay(1000, stoppingToken);
				}
			}
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			// Service stoped
			_logger.LogInformation("Stopping Signal bot at: {time}", DateTimeOffset.Now);
			await base.StopAsync(cancellationToken);
		}
	}
}
