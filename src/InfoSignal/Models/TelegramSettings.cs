namespace InfoSignal.Models
{
	public class TelegramSettings
	{
		/// <summary>
		/// Telegram token.
		/// </summary>
		public required string Token { get; set; }

		/// <summary>
		/// Telegram chat id.
		/// </summary>
		public required string ChatId { get; set; }
	}
}
