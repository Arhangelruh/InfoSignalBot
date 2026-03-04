using InfoSignal;
using InfoSignal.Models;
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