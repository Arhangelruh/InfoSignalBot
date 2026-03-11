namespace InfoSignal.Interfaces
{
	public interface IMailService
	{
		/// <summary>
		/// Send message.
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="ct">Cancellation token</param>
		/// <returns></returns>
		Task SendAsync(string message, CancellationToken ct);
	}
}
