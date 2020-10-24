using System;

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
}