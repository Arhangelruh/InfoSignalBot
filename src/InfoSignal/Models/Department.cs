namespace InfoSignal.Models
{
	public class Department
	{
		/// <summary>
		/// Department id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Department code.
		/// </summary>
		public required string Code {  get; set; }

		/// <summary>
		/// Department name.
		/// </summary>
    	public required string Name { get; set; }

		/// <summary>
		/// List currency amounts.
		/// </summary>
		public List<CurrencyAmount> CurrencyAmounths { get; set; } = [];
	}
}
