namespace InfoSignal.Models
{
	public class MailSettings
	{
		/// <summary>
		/// Server addres.
		/// </summary>
		public string? SMTPServer { get; set; }

		/// <summary>
		/// Server port.
		/// </summary>
		public string? SMTPPort { get; set; }

		/// <summary>
		/// Login for SMTP.
		/// </summary>
		public string? SMTPLogin { get; set; }

		/// <summary>
		/// Password for SMTP.
		/// </summary>
		public string? SMTPPassword { get; set; }

		/// <summary>
		/// List of receivers.
		/// </summary>
		public string? Sender { get; set; }

		/// <summary>
		/// List of receivers.
		/// </summary>
		public List<string>? Receivers { get; set; }
	}
}
