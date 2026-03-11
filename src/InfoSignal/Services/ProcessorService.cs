using InfoSignal.Constants;
using InfoSignal.Interfaces;

namespace InfoSignal.Services
{
	public class ProcessorService(IApiService apiService,
		ILogger<ProcessorService> logger,
		DepartmentsService departments,
		IMailService mailService,
		TelegramQueue telegramQueue
		) : IProcessorService
	{
		private readonly IApiService _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
		private readonly ILogger<ProcessorService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
		private readonly DepartmentsService _departments = departments ?? throw new ArgumentNullException(nameof(departments));
		private readonly IMailService _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
		private readonly TelegramQueue _telegramQueue = telegramQueue ?? throw new ArgumentNullException(nameof(telegramQueue));

		public async Task Process(CancellationToken ct)
		{
			try
			{
				var response = await _apiService.GetAmountsAsync();

				try
				{
					foreach (var depApi in response.DepartmentAmounts)
					{
						var department = _departments.GetById(depApi.DepartmentID);
						if (department != null)
						{

							if (department.CurrencyAmounths.Count > 0)
							{
								foreach (var rate in depApi.CurrencyAmounts)
								{
									var checkOldCurrency = department.CurrencyAmounths.FirstOrDefault(r => r.Currency == rate.Currency);
									if (checkOldCurrency != null)
									{
										if (rate.Amount != checkOldCurrency.Amount)
										{
											string picture;
											if (CurrencyPictureDictionary.Pictures.TryGetValue(rate.Currency, out var value))
											{
												picture = value;
											}
											else
											{
												picture = "";
											}

											if (checkOldCurrency.Amount > 2 && rate.Amount <= 2)
											{											
												await _telegramQueue.EnqueueAsync($"Обратите внимание в {department.Name} заканчиваются {rate.Currency} {picture}");
											}
											if (checkOldCurrency.Amount <= 2 && rate.Amount > 2)
											{
												await _telegramQueue.EnqueueAsync($"Количество {rate.Currency} {picture} в {department.Name} пришло в норму.");
											}

											_departments.UpdateCurrencyAmounth(department.Id, rate);											
										}										
									}
									else
									{
										_departments.AddCurrencyAmounth(department.Id, rate);
									}
								}
							}
							else
							{
								_departments.AddCurrencyAmounths(
									department.Id,
									depApi.CurrencyAmounts
								);
							}
						}
						else
						{
							_logger.LogWarning("There is new department in API.");
							await _mailService.SendAsync(
								$"По API передается депортамент с ID: {depApi.DepartmentID} которого нет в конфигурационном файле {DictionaryPath.PathToDepartmentsList}." +
								$"\nНеобходимо добавить департамент и перезапустить сервис.", ct);
						}
					}
				}
				catch
				{
					throw;
				}
			}
			catch
			{
				_logger.LogWarning("Can't get departments data from API");
				await _mailService.SendAsync("Ошибка получения данных по API", ct);
			}
		}
	}
}
