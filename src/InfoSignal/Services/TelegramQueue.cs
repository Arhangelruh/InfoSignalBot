using System.Threading.Channels;

namespace InfoSignal.Services
{
	public class TelegramQueue
	{
		private readonly Channel<string> _queue = Channel.CreateUnbounded<string>();

		public ValueTask EnqueueAsync(string message)
		{
			return _queue.Writer.WriteAsync(message);
		}

		public ValueTask<string> DequeueAsync(CancellationToken ct)
		{
			return _queue.Reader.ReadAsync(ct);
		}
	}
}
