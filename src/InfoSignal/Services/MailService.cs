using InfoSignal.Interfaces;
using InfoSignal.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace InfoSignal.Services
{
	///<inheritdoc/>
	public class MailService(IOptions<MailSettings> options, ILogger<MailService> logger) : IMailService
	{
		private readonly MailSettings _options = options.Value;
		private readonly ILogger<MailService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

		public async Task SendAsync(string message, CancellationToken ct)
		{
			if (_options.SMTPServer != "" && _options.SMTPPort != "" && _options.Sender != "")
			{
				MailAddress from = new(_options.Sender);
				using SmtpClient smtp = new(_options.SMTPServer);

				smtp.Port = int.Parse(_options.SMTPPort);

				if (_options.SMTPLogin != null && _options.SMTPPassword != null)
				{
					smtp.Credentials = new NetworkCredential(_options.SMTPLogin, _options.SMTPPassword);

					if (_options.Receivers != null)
					{
						MailAddress to = new(_options.Receivers[0]);
						using MailMessage mess = new(from, to);

						if (_options.Receivers.Count > 0)
						{
							foreach (var email in _options.Receivers)
							{
								mess.CC.Add(email);
							}

							mess.Subject = "Бот количества наличных в кассах";
							mess.Body = message;

							await smtp.SendMailAsync(mess, ct);
						}
						else
						{
							_logger.LogWarning("There is no one reciver to sending email.");
						}
					}
					else
					{
						_logger.LogWarning("There is no one reciver to sending email.");
					}
				}
				else
				{
					_logger.LogError("Please check email configuration Login or Password.");
				}
			}
			else
			{
				_logger.LogError("Please check email configuration port, server or sender address.");
			}
		}
	}
}
