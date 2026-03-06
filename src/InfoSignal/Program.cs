using InfoSignal;
using InfoSignal.Interfaces;
using InfoSignal.Models;
using InfoSignal.Services;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

var logger = LogManager.Setup()
	.LoadConfigurationFromAppSettings()
	.GetCurrentClassLogger();

try
{
	logger.Info("Starting Signal bot service");

	var builder = Host.CreateApplicationBuilder(args);
	builder.Logging.ClearProviders();
	builder.Logging.AddNLog();

	builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
	builder.Services.Configure<TimeSettings>(builder.Configuration.GetSection("TimeSettings"));
	builder.Services.Configure<APISettings>(builder.Configuration.GetSection("APISettings"));

	builder.Services.AddSingleton<DepartmentsService>();
	builder.Services.AddSingleton<IMailService, MailService>();

	builder.Services.AddHostedService<Worker>();

	var host = builder.Build();
	host.Run();
}
catch (Exception ex)
{
	logger.Error(ex, "Application stopped due to exception");
	throw;
}
finally
{ LogManager.Shutdown(); }