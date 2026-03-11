using InfoSignal.Constants;
using InfoSignal.Models;
using System.Text.Json;

namespace InfoSignal.Services
{
	public class DepartmentsService
	{
		private readonly Dictionary<int, Department> _departments;		

		private ILogger<DepartmentsService> _logger;

		private readonly object _lock = new();

		/// <summary>
		/// Initialize departments list,
		/// </summary>
		/// <param name="logger"></param>
		public DepartmentsService(ILogger<DepartmentsService> logger) {
			_logger = logger;
			try
			{				
				string jsonString = File.ReadAllText(DictionaryPath.PathToDepartmentsList);
				var CurrentDepartments = JsonSerializer.Deserialize<List<Department>>(jsonString);
				_departments = CurrentDepartments.ToDictionary(x=>x.Id);
			}
			catch (DirectoryNotFoundException)
			{
				_logger.LogCritical($"Directory {DictionaryPath.PathToDepartmentsList} wasn't found.");
				throw;
			}
		}

		/// <summary>
		/// Get all departments.
		/// </summary>
		/// <returns>Department collection.</returns>
		public IReadOnlyCollection<Department> GetAll()
		{
			return _departments.Values;
		}

		/// <summary>
		/// Get department by id.
		/// </summary>
		/// <param name="id">Department Id</param>
		/// <returns>Department model.</returns>
		public Department? GetById(int id)
		{
			_departments.TryGetValue(id, out var dep);
			return dep;
		}

		/// <summary>
		/// Add new list currency amounts.
		/// </summary>
		/// <param name="id">Department id.</param>
		/// <param name="currencyAmounts">Currencyamounts collection.</param>
		public void AddCurrencyAmounths(int id, List<CurrencyAmount> currencyAmounts)
		{
			lock (_lock)
			{
				if (_departments.TryGetValue(id, out var dep))
				{
					dep.CurrencyAmounths = currencyAmounts;
				}
			}
		}

		/// <summary>
		/// Add new currency amount.
		/// </summary>
		/// <param name="id">Department id.</param>
		/// <param name="currencyAmount">Currencyamount model.</param>
		public void AddCurrencyAmounth(int id, CurrencyAmount currencyAmount)
		{
			lock (_lock)
			{
				if (_departments.TryGetValue(id, out var dep))
				{
					dep.CurrencyAmounths.Add(currencyAmount);
				}
			}
		}

		/// <summary>
		/// Update currency amount.
		/// </summary>
		/// <param name="id">Department id.</param>
		/// <param name="currencyAmount">Currencyamount model.</param>
		public void UpdateCurrencyAmounth(int id, CurrencyAmount currencyAmount)
		{
			lock (_lock) {
				if (_departments.TryGetValue(id, out var dep))
				{ 
					var rate = dep.CurrencyAmounths.FirstOrDefault(r=>r.Currency == currencyAmount.Currency);
					rate?.Amount = currencyAmount.Amount;
				}
			}
		}
	}
}
