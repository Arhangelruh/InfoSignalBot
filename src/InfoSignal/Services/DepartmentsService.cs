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

		public IReadOnlyCollection<Department> GetAll()
		{
			return _departments.Values;
		}

		public Department? GetById(int id)
		{
			_departments.TryGetValue(id, out var dep);
			return dep;
		}

		public void UpdateValue(int id, List<CurrencyAmount> currencyAmounts)
		{
			lock (_lock)
			{
				if (_departments.TryGetValue(id, out var dep))
				{
					dep.CurrencyAmounths = currencyAmounts;
				}
			}
		}
	}
}
