using Flurl;
using Flurl.Http;
using InfoSignal.Interfaces;
using InfoSignal.Models;
using Microsoft.Extensions.Options;

namespace InfoSignal.Services
{
	///<inheritdoc/>
	public class ApiService(IOptions<APISettings> options, ILogger<ApiService> logger) : IApiService
	{
		private readonly APISettings _options = options.Value;
		private readonly ILogger<ApiService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

		public async Task<DepartmentAmountsResponse> GetAmountsAsync()
		{
			if (_options.CurrencyAPILink == null || _options.CurrencyAPILink == string.Empty)
				_logger.LogCritical("Link to currency API is null or empty");

			if (_options.Login == null || _options.Login == string.Empty)
				_logger.LogCritical("Currency API Login is null or empty");

			if (_options.Password == null || _options.Password == string.Empty)
				_logger.LogCritical("Currency API Password is null or empty");

			return await _options.CurrencyAPILink
					.SetQueryParams(new
					{
						extclientid = 1,
						extsystemid = "coinsBot"
					})
					.WithBasicAuth(_options.Login, _options.Password)
					.GetJsonAsync<DepartmentAmountsResponse>();			
		}
	}
}
