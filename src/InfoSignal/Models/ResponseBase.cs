public class ResponseBase
{
	/// <summary>
	/// Result.
	/// </summary>
	public string Result { get; set; }

	/// <summary>
	/// Error code.
	/// </summary>
	public string ErrorCode { get; set; }

	/// <summary>
	/// Error message.
	/// </summary>
	public string Message { get; set; }

	/// <summary>
	/// Request reference.
	/// </summary>
	public string DocRefNumber { get; set; }

	/// <summary>
	/// Request date.
	/// </summary>
	public DateTime DocDateTime { get; set; }

	public string FeeAmount { get; set; }
	public object PrintForms { get; set; }
	public object CardAmounts { get; set; }
	public object AcceptCode { get; set; }
	public object CorrelationId { get; set; }
}