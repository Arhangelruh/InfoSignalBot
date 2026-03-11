using InfoSignal.Interfaces;
using InfoSignal.Models;
using Microsoft.Extensions.Options;

namespace InfoSignal.Services
{
	public class TelegramService(IOptions<TelegramSettings> options, HttpClient httpClient):ITelegramService
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly TelegramSettings _options = options.Value;

		public async Task SendAsync(string message, CancellationToken ct)
		{
			var url =
				$"https://api.telegram.org/bot{_options.Token}/sendMessage";

			var content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				["chat_id"] = _options.ChatId,
				["text"] = message
			});

			var response = await _httpClient.PostAsync(url, content, ct);
			var body = await response.Content.ReadAsStringAsync(ct);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(body);
			}
		}
	}
}
