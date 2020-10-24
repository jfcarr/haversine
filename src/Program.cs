using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using Haversine.Helpers;
using Haversine.Lib;
using Haversine.Models;

namespace Haversine
{
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

			// Distance calculations
			double result1 = GeoCalc.Distance(pos1, pos2);
			double result2 = GeoCalc.Distance(pos1, pos3);
			double result3 = GeoCalc.Distance(pos1, pos4);

			Console.WriteLine($"Distance between {pos1.Name} and {pos2.Name} is {result1.RoundTo(2)} miles ({result1.ToKilometers().RoundTo(2)} kilometers).");
			Console.WriteLine($"Distance between {pos1.Name} and {pos3.Name} is {result2.RoundTo(2)} miles ({result2.ToKilometers().RoundTo(2)} kilometers).");
			Console.WriteLine($"Distance between {pos1.Name} and {pos4.Name} is {result3.RoundTo(2)} miles ({result3.ToKilometers().RoundTo(2)} kilometers).");

			Console.WriteLine();

			// Calculate bounding rectangle
			double boundingRadiusMiles = 50;
			var geoArea = GeoCalc.BoundingCoordinates(pos1, boundingRadiusMiles);

			Console.WriteLine($"The bounding coordinates for a center point at {pos1.Name} ({pos1.Latitude.RoundTo(2)}, {pos1.Longitude.RoundTo(2)}), with a radius of {boundingRadiusMiles} miles, are as follows:");
			Console.WriteLine($"\tMinimum and maximum latitude values are {geoArea.MinimumLatitude.RoundTo(2)}d and {geoArea.MaximumLatitude.RoundTo(2)}d, respectively.");
			Console.WriteLine($"\tMinimum and maximum longitude values are {geoArea.MinimumLongitude.RoundTo(2)}d and {geoArea.MaximumLongitude.RoundTo(2)}d, respectively.");

			Console.WriteLine();

			// Generate list of cities within a specified radius of an origin point
			var withinDistance = 10;
			var cities = GeoCalc.CitiesWithinDistance(pos1, withinDistance);
			Console.WriteLine($"Cities within {withinDistance} miles of {pos1.Name}");

			IEnumerable<Models.City> query = from city in cities orderby city.Distance select city;
			foreach (var city in query)
				Console.WriteLine($"\t{city.CityName}, {city.StateName} in {city.CountyName} county ({city.Distance.RoundTo(2)} miles)");
		}
	}
}
