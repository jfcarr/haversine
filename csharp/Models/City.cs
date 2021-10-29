namespace Haversine.Models
{
	public class City
	{
		public string CityName { get; set; }
		public string StateAbbreviation { get; set; }
		public string StateName { get; set; }
		public string CountyName { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Distance { get; set; }
	}
}