namespace InfoSignal.Interfaces
{
	public interface IProcessorService
	{
		/// <summary>
		/// Main logic.
		/// </summary>
		/// <param name="ct">Cancelliation token.</param>
		/// <returns></returns>
		Task Process(CancellationToken ct);
	}
}
