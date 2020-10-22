﻿using System;

namespace Haversine
{
	/// <summary>
	/// Coordinate information for a position.
	/// </summary>
	public struct Position
	{
		public string Name;
		public double Latitude;
		public double Longitude;
	}

	/// <summary>
	/// Coordinate information for an area.
	/// </summary>
	public struct GeoArea
	{
		public double MinimumLatitude;
		public double MaximumLatitude;
		public double MinimumLongitude;
		public double MaximumLongitude;
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

			double result1 = GeoCalc.Distance(pos1, pos2);
			double result2 = GeoCalc.Distance(pos1, pos3);
			double result3 = GeoCalc.Distance(pos1, pos4);

			Console.WriteLine($"Distance between {pos1.Name} and {pos2.Name} is {result1.RoundTo(2)} miles ({result1.ToKilometers().RoundTo(2)} kilometers).");
			Console.WriteLine($"Distance between {pos1.Name} and {pos3.Name} is {result2.RoundTo(2)} miles ({result2.ToKilometers().RoundTo(2)} kilometers).");
			Console.WriteLine($"Distance between {pos1.Name} and {pos4.Name} is {result3.RoundTo(2)} miles ({result3.ToKilometers().RoundTo(2)} kilometers).");

			Console.WriteLine();

			double boundingRadiusMiles = 50;
			var geoArea = GeoCalc.BoundingCoordinates(pos1, boundingRadiusMiles);

			Console.WriteLine($"The bounding coordinates for a center point at {pos1.Name} ({pos1.Latitude.RoundTo(2)}, {pos1.Longitude.RoundTo(2)}), with a radius of {boundingRadiusMiles} miles, are as follows:");
			Console.WriteLine($"\tMinimum and maximum latitude values are {geoArea.MinimumLatitude.RoundTo(2)}d and {geoArea.MaximumLatitude.RoundTo(2)}d, respectively.");
			Console.WriteLine($"\tMinimum and maximum longitude values are {geoArea.MinimumLongitude.RoundTo(2)}d and {geoArea.MaximumLongitude.RoundTo(2)}d, respectively.");
		}
	}

	static class GeoCalc
	{
		public static double EarthRadius { get { return 6371.01; } }
		public static double MinimumLatitude { get { return (-90d).ToRadians(); } }
		public static double MaximumLatitude { get { return (90d).ToRadians(); } }
		public static double MinimumLongitude { get { return (-180d).ToRadians(); } }
		public static double MaximumLongitude { get { return (180d).ToRadians(); } }

		/// <summary>
		/// Distance, in miles, between two points.  Calculated using the Haversine formula.
		/// </summary>
		/// <param name="startingPoint"></param>
		/// <param name="endingPoint"></param>
		/// <returns></returns>
		public static double Distance(Position startingPoint, Position endingPoint)
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
		/// Calculate bounding coordinates for a position.
		/// </summary>
		/// <param name="centerPoint"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static GeoArea BoundingCoordinates(Position centerPoint, double distance)
		{

			double radLat = centerPoint.Latitude.ToRadians();
			double radLon = centerPoint.Longitude.ToRadians();

			if (distance < 0d)
				throw new Exception("Distance cannot be less than 0");

			// angular distance in radians on a great circle
			double radDist = distance / EarthRadius;

			double minLat = radLat - radDist;
			double maxLat = radLat + radDist;

			double minLon, maxLon;
			if (minLat > MinimumLatitude && maxLat < MaximumLatitude)
			{
				double deltaLon = Math.Asin(Math.Sin(radDist) / Math.Cos(radLat));
				minLon = radLon - deltaLon;
				if (minLon < MinimumLongitude)
					minLon += 2d * Math.PI;
				maxLon = radLon + deltaLon;
				if (maxLon > MaximumLongitude)
					maxLon -= 2d * Math.PI;
			}
			else
			{
				// a pole is within the distance
				minLat = Math.Max(minLat, MinimumLatitude);
				maxLat = Math.Min(maxLat, MaximumLatitude);
				minLon = MinimumLongitude;
				maxLon = MaximumLongitude;
			}

			return new GeoArea()
			{
				MinimumLatitude = minLat.ToDegrees(),
				MaximumLatitude = maxLat.ToDegrees(),
				MinimumLongitude = minLon.ToDegrees(),
				MaximumLongitude = maxLon.ToDegrees()
			};
		}
	}

	static class Extensions
	{
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
		/// Convert radians to degrees
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static double ToDegrees(this double val)
		{
			return val * (180.0 / Math.PI);
		}

		/// <summary>
		/// Convert degrees to radians.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static double ToRadians(this double val)
		{
			return (Math.PI / 180) * val;
		}
	}

}