using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vaxometer_DataRefresh.Models.GeoNames
{
	[XmlRoot(ElementName = "geonames")]
	public class Geonames
	{
		[XmlElement(ElementName = "code")]
		public List<Code> Code { get; set; }
	}

	[XmlRoot(ElementName = "code")]
	public class Code
	{
		[XmlElement(ElementName = "postalcode")]
		public long Postalcode { get; set; }
		[XmlElement(ElementName = "name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "countryCode")]
		public string CountryCode { get; set; }
		[XmlElement(ElementName = "lat")]
		public string Lat { get; set; }
		[XmlElement(ElementName = "lng")]
		public string Lng { get; set; }
		[XmlElement(ElementName = "adminCode1")]
		public string AdminCode1 { get; set; }
		[XmlElement(ElementName = "adminName1")]
		public string AdminName1 { get; set; }
		[XmlElement(ElementName = "adminCode2")]
		public string AdminCode2 { get; set; }
		[XmlElement(ElementName = "adminName2")]
		public string AdminName2 { get; set; }
		[XmlElement(ElementName = "adminCode3")]
		public string AdminCode3 { get; set; }
		[XmlElement(ElementName = "adminName3")]
		public string AdminName3 { get; set; }
		[XmlElement(ElementName = "distance")]
		public string Distance { get; set; }
	}
}
