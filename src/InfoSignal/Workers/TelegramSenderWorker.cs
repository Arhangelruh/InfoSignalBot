using InfoSignal.Interfaces;
using InfoSignal.Services;
using System.Text.Json;

namespace InfoSignal.Workers
{
	public class TelegramSenderWorker : BackgroundService
	{
		private readonly TelegramQueue _queue;
		private readonly ITelegramService _telegram;
		private readonly ILogger<TelegramSenderWorker> _logger;

		private static readonly TimeSpan DefaultDelay = TimeSpan.FromSeconds(1.1);

		public TelegramSenderWorker(
			TelegramQueue queue,
			ITelegramService telegram,
			ILogger<TelegramSenderWorker> logger)
		{
			_queue = queue;
			_telegram = telegram;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var message = await _queue.DequeueAsync(stoppingToken);

				try
				{					
					await SendWithRetry(message, stoppingToken);
					await Task.Delay(DefaultDelay, stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Telegram send error");
				}
			}
		}

		private async Task SendWithRetry(string message, CancellationToken ct)
		{
			int attempt = 0;

			while (attempt < 5 && !ct.IsCancellationRequested)
			{
				try
				{
					await _telegram.SendAsync(message, ct);
					return;
				}
				catch (Exception ex)
				{
					attempt++;

					var retryAfter = TryParseRetryAfter(ex);

					if (retryAfter != null)
					{
						_logger.LogWarning(
							"Telegram rate limit. Retry after {sec}s",
							retryAfter);

						await Task.Delay(
							TimeSpan.FromSeconds(retryAfter.Value), ct);

						continue;
					}

					_logger.LogError(ex,
						"Telegram send failed (attempt {attempt})",
						attempt);

					await Task.Delay(TimeSpan.FromSeconds(3), ct);
				}
			}

			_logger.LogError("Telegram message dropped after retries: {msg}", message);
		}

		private int? TryParseRetryAfter(Exception ex)
		{
			try
			{
				var jsonStart = ex.Message.IndexOf('{');

				if (jsonStart < 0)
					return null;

				var json = ex.Message.Substring(jsonStart);

				using var doc = JsonDocument.Parse(json);

				if (doc.RootElement.TryGetProperty("parameters", out var p) &&
					p.TryGetProperty("retry_after", out var r))
				{
					return r.GetInt32();
				}
			}
			catch { }

			return null;
		}
	}
}
