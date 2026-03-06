using InfoSignal.Models;

namespace InfoSignal.Interfaces
{
	public interface IApiService
	{
		/// <summary>
		/// Get amount statistic.
		/// </summary>
		/// <returns>DepartmentAmountsResponse</returns>
		Task<DepartmentAmountsResponse> GetAmountsAsync();		
	}
}
