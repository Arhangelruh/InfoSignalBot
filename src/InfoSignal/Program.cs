using InfoSignal.Interfaces;
using InfoSignal.Models;
using InfoSignal.Services;
using InfoSignal.Workers;
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
	builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("TelegramSettings"));

	builder.Services.AddSingleton<DepartmentsService>();
	builder.Services.AddSingleton<IMailService, MailService>();
	builder.Services.AddSingleton<IApiService, ApiService>();
	builder.Services.AddSingleton<TelegramQueue>();
	builder.Services.AddSingleton<IProcessorService, ProcessorService>();

	builder.Services.AddHostedService<Worker>();
	builder.Services.AddHostedService<TelegramSenderWorker>();
	builder.Services.AddHttpClient<ITelegramService, TelegramService>();

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