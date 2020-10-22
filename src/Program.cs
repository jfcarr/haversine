using System;

namespace Haversine
{
	/// <summary>
	/// Coordinate information.
	/// </summary>
	public struct Position
	{
		public string Name;
		public double Latitude;
		public double Longitude;
	}

	class Program
	{
		static void Main(string[] args)
		{
			var pos1 = new Position()
			{
				Name = "West Alexandria, OH",
				Latitude = 39.744596,
				Longitude = -84.533226
			};

			var pos2 = new Position()
			{
				Name = "Tipp City, OH",
				Latitude = 39.958044,
				Longitude = -84.173374
			};

			var pos3 = new Position()
			{
				Name = "Lubbock, TX",
				Latitude = 33.576001,
				Longitude = -101.858604
			};

			var pos4 = new Position()
			{
				Name = "Paris, France",
				Latitude = 48.856113,
				Longitude = 2.351508
			};

			double result1 = pos1.DistanceTo(pos2);
			double result2 = pos1.DistanceTo(pos3);
			double result3 = pos1.DistanceTo(pos4);

			Console.WriteLine($"Distance between {pos1.Name} and {pos2.Name} is {result1.RoundTo(2)} miles ({result1.ToKilometers().RoundTo(2)} kilometers).");
			Console.WriteLine($"Distance between {pos1.Name} and {pos3.Name} is {result2.RoundTo(2)} miles ({result2.ToKilometers().RoundTo(2)} kilometers).");
			Console.WriteLine($"Distance between {pos1.Name} and {pos4.Name} is {result3.RoundTo(2)} miles ({result3.ToKilometers().RoundTo(2)} kilometers).");
		}
	}

	static class Extensions
	{
		/// <summary>
		/// Distance, in miles, between two points.  Calculated using the Haversine formula.
		/// </summary>
		/// <param name="startingPoint"></param>
		/// <param name="endingPoint"></param>
		/// <returns></returns>
		public static double DistanceTo(this Position startingPoint, Position endingPoint)
		{
			double R = 3960;

			double dLat = (endingPoint.Latitude - startingPoint.Latitude).ToRadians();
			double dLon = (endingPoint.Longitude - startingPoint.Longitude).ToRadians();

			double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(startingPoint.Latitude.ToRadians()) * Math.Cos(endingPoint.Latitude.ToRadians()) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
			double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
			double d = R * c;

			return d;
		}

		/// <summary>
		/// Round a value to a set number of decimal places.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="decimalPlaces"></param>
		/// <returns></returns>
		public static double RoundTo(this double value, int decimalPlaces)
		{
			return Math.Round(value, decimalPlaces);
		}

		/// <summary>
		/// Convert miles to kilometers.
		/// </summary>
		/// <param name="miles"></param>
		/// <returns></returns>
		public static double ToKilometers(this double miles)
		{
			return miles * 1.609344;
		}

		/// <summary>
		/// Convert to radians.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static double ToRadians(this double val)
		{
			return (Math.PI / 180) * val;
		}
	}

}
