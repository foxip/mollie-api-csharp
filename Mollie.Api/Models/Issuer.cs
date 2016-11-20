namespace Mollie.Api.Models
{

	#region Enums

    #endregion

	#region Plain objects

    /// <summary>
	/// iDeal issuer
	/// </summary>
	public class Issuer
	{
		public string id { get; set; }
		public string name { get; set; }
		public string method { get; set; }
	}

    #endregion
}
