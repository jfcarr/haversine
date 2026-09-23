using System;
using System.Linq;
using System.Data.SQLite;
using System.Reflection;

namespace Haversine.Helpers
{
	public static class Extensions
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
		/// <param name="radians"></param>
		/// <returns></returns>
		public static double ToDegrees(this double radians)
		{
			return radians * (180.0 / Math.PI);
		}

		/// <summary>
		/// Convert degrees to radians.
		/// </summary>
		/// <param name="degrees"></param>
		/// <returns></returns>
		public static double ToRadians(this double degrees)
		{
			return (Math.PI / 180) * degrees;
		}
	}

	public static class SQLiteExtensions
	{
		/// <summary>
		/// Get a reference to a column, and return its value as a string.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="columnName"></param>
		public static string GetColumnString(this SQLiteDataReader reader, string columnName)
		{
			return reader.GetString(reader.GetOrdinal(columnName));
		}

		/// <summary>
		/// Get a reference to a column, and return its value as a double.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public static double GetColumnDouble(this SQLiteDataReader reader, string columnName)
		{
			return Convert.ToDouble(reader.GetString(reader.GetOrdinal(columnName)));
		}

		/// <summary>
		/// Iterate the properties of the City class, and set each to the value of a data reader column having the same name.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static Models.City MapToCityModel(this SQLiteDataReader reader)
		{
			var city = new Models.City();

			foreach (var propertyInfo in city.GetType().GetProperties())
			{
				if (!propertyInfo.Name.Equals("Distance"))
				{
					if (propertyInfo.PropertyType == typeof(double))
					{
						propertyInfo.SetValue(city, GetColumnDouble(reader, propertyInfo.Name));
					}

					if (propertyInfo.PropertyType == typeof(string))
					{
						propertyInfo.SetValue(city, GetColumnString(reader, propertyInfo.Name));
					}
				}
			}

			return city;
		}
	}
}