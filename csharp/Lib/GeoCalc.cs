using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using Haversine.Helpers;
using Haversine.Models;

namespace Haversine.Lib
{
	public static class GeoCalc
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

		/// <summary>
		/// Generate list of cities within a given radius of an origin point.
		/// </summary>
		/// <param name="centerPoint"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static List<Models.City> CitiesWithinDistance(Position centerPoint, double distanceLimit)
		{
			var inputCities = new List<Models.City>();
			var outputCities = new List<Models.City>();

			var connectionString = $"URI=file:{Path.Combine("database", "uscities.db")}";

			using (var connection = new SQLiteConnection(connectionString))
			{
				connection.Open();

				var statement = $"SELECT city AS CityName,state_id AS StateAbbreviation,state_name as StateName,county_name as CountyName,lat as Latitude,lng as Longitude FROM uscities";

				using (var command = new SQLiteCommand(statement, connection))
				{
					using (SQLiteDataReader reader = command.ExecuteReader())
					{
						if (reader.HasRows)
						{
							while (reader.Read())
							{
								var inputCity = reader.MapToCityModel();

								var destinationPoint = new Position()
								{
									Name = inputCity.CityName,
									Latitude = inputCity.Latitude,
									Longitude = inputCity.Longitude
								};

								inputCity.Distance = Distance(centerPoint, destinationPoint);

								inputCities.Add(inputCity);
							}

							outputCities = inputCities
								.Where(x => x.Distance <= distanceLimit)
								.OrderBy(x => x.Distance)
								.Select(x => x)
								.ToList<City>();
						}
					}
				}
			}

			return outputCities;
		}
	}
}