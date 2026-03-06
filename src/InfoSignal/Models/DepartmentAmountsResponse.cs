namespace InfoSignal.Models
{
	public class DepartmentAmountsResponse
	{
		/// <summary>
		/// List Departmentamounts.
		/// </summary>
		public List<DepartmentAmount> DepartmentAmounts { get; set; }

		/// <summary>
		/// Additional information about response.
		/// </summary>
		public ResponseBase ResponseBase { get; set; }
	}
}
