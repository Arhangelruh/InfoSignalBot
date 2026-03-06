using Flurl.Http;
using InfoSignal.Constants;
using InfoSignal.Interfaces;
using InfoSignal.Models;
using Microsoft.Extensions.Options;

namespace InfoSignal.Services
{
	///<inheritdoc/>
	public class ApiService(IOptions<APISettings> options) : IApiService
	{
		private readonly APISettings _options = options.Value;

		public async Task<DepartmentAmountsResponse> GetAmountsAsync()
		{
			return await ApiLinks.getCurrencies
				.WithHeader("extclientid", "1")
				.WithHeader("extsystemid", "coinsBot")
				.WithBasicAuth(_options.Login, _options.Password)
				.GetJsonAsync<DepartmentAmountsResponse>();				
		}
	}
}
