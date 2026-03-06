namespace InfoSignal.Models
{
	public class DepartmentAmount
	{
		/// <summary>
		/// Department id.
		/// </summary>
		public int DepartmentID { get; set; }

		/// <summary>
		/// List currencyamounts.
		/// </summary>
		public List<CurrencyAmount> CurrencyAmounts { get; set; }
	}
}
