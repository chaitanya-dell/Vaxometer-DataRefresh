using System.Collections.Generic;

namespace Vaxometer_DataRefresh.Models.GeoNames
{
	public class GeoNamePinCode
	{
		public List<PostalCode> PostalCodes { get; set; }
	}

	public class PostalCode
	{
		
		public long Postalcode { get; set; }
		public string CountryCode { get; set; }
		public string Lat { get; set; }
		public string Lng { get; set; }
		public string AdminCode1 { get; set; }
		public string AdminName1 { get; set; }
		public string AdminCode2 { get; set; }
		public string AdminName2 { get; set; }
		public string AdminCode3 { get; set; }
		public string AdminName3 { get; set; }
	}
}
