using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AdminPanel.Shared
{
	public static class Extensions
	{
		public static string ConvertToQueryString<T>(T entity) where T : class
		{
			var props = typeof(T).GetProperties();

			return $"?{string.Join('&', props.Where(r => r.GetValue(entity) != null).Select(r => $"{HttpUtility.UrlEncode(r.Name)}={HttpUtility.UrlEncode(r.GetValue(entity).ToString())}"))}";
		}

		public static DateTime LocalTime(string timeZoneId)
		{
			try
			{
				TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
				return TimeZoneInfo.ConvertTime(DateTime.UtcNow, destinationTimeZone);
			}
			catch
			{
				return DateTime.UtcNow;
			}
		}

		public static DateTime ConvertToLocalTime(this DateTime datetime, string timeZoneId)
		{
			try
			{
				TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
				return TimeZoneInfo.ConvertTime(datetime, destinationTimeZone);
			}
			catch
			{
				return DateTime.UtcNow;
			}
		}

		public static IEnumerable<TSource> DistinctByKey<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			foreach (var item in list)
			{
				action.Invoke(item);
			}
		}

		public static string StripHtml(this string input)
		{
			// Will this simple expression replace all tags???
			var tagsExpression = new Regex(@"</?.+?>");
			return tagsExpression.Replace(input, " ");
		}

		/// <summary>
		/// Truncates the string to a specified length and replace the truncated to a ...
		/// </summary>
		/// <param name="text">string that will be truncated</param>
		/// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
		/// <returns>truncated string</returns>
		public static string Truncate(this string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}

			// replaces the truncated string to a ...
			const string suffix = "...";
			string truncatedString = text;

			if (maxLength <= 0) return truncatedString;
			int strLength = maxLength - suffix.Length;

			if (strLength <= 0) return truncatedString;

			if (text == null || text.Length <= maxLength) return truncatedString;

			truncatedString = text.Substring(0, strLength);
			truncatedString = truncatedString.TrimEnd();
			truncatedString += suffix;
			return truncatedString;
		}

		#region Exception.ToLogString
		/// <summary>
		/// <para>Creates a log-string from the Exception.</para>
		/// <para>The result includes the stacktrace, innerexception et cetera, separated by <seealso cref="Environment.NewLine"/>.</para>
		/// </summary>
		/// <param name="ex">The exception to create the string from.</param>
		/// <param name="additionalMessage">Additional message to place at the top of the string, maybe be empty or null.</param>
		/// <returns></returns>
		public static string ToLogString(this Exception ex, string additionalMessage)
		{
			StringBuilder msg = new StringBuilder();

			if (!string.IsNullOrEmpty(additionalMessage))
			{
				msg.Append(additionalMessage);
				msg.Append(Environment.NewLine);
			}

			if (ex != null)
			{
				try
				{
					Exception orgEx = ex;

					msg.Append("Exception:");
					msg.Append(Environment.NewLine);
					while (orgEx != null)
					{
						msg.Append(orgEx.Message);
						msg.Append(Environment.NewLine);
						orgEx = orgEx.InnerException;
					}

					if (ex.Data != null)
					{
						foreach (object i in ex.Data)
						{
							msg.Append("Data :");
							msg.Append(i.ToString());
							msg.Append(Environment.NewLine);
						}
					}

					if (ex.StackTrace != null)
					{
						msg.Append("StackTrace:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.StackTrace.ToString());
						msg.Append(Environment.NewLine);
					}

					if (ex.Source != null)
					{
						msg.Append("Source:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.Source);
						msg.Append(Environment.NewLine);
					}

					if (ex.TargetSite != null)
					{
						msg.Append("TargetSite:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.TargetSite.ToString());
						msg.Append(Environment.NewLine);
					}

					Exception baseException = ex.GetBaseException();
					if (baseException != null)
					{
						msg.Append("BaseException:");
						msg.Append(Environment.NewLine);
						msg.Append(ex.GetBaseException());
					}
				}
				finally
				{
				}
			}
			return msg.ToString();
		}
		#endregion Exception.ToLogString

		/// <summary>
		/// Converts <see cref="TimeSpan"/> objects to a simple human-readable string.  Examples: 3.1 seconds, 2 minutes, 4.23 hours, etc.
		/// </summary>
		/// <param name="span">The timespan.</param>
		/// <param name="significantDigits">Significant digits to use for output.</param>
		/// <returns></returns>
		public static string ToHumanTimeString(this TimeSpan span, int significantDigits = 3)
		{
			var format = "G" + significantDigits;
			return span.TotalMilliseconds < 1000 ? span.TotalMilliseconds.ToString(format) + " milliseconds"
				: (span.TotalSeconds < 60 ? span.TotalSeconds.ToString(format) + " seconds"
					: (span.TotalMinutes < 60 ? span.TotalMinutes.ToString(format) + " minutes"
						: (span.TotalHours < 24 ? span.TotalHours.ToString(format) + " hours"
												: span.TotalDays.ToString(format) + " days")));
		}

		public static bool IsNotNullOrEmpty(this string input)
		{
			return !String.IsNullOrEmpty(input);
		}

		/// <summary>
		/// Converts a UTC time to a time in the given time zone
		/// </summary>
		/// <param name="targetTimeZone">The time zone to convert to</param>
		/// <param name="utcTime">The UTC time</param>
		/// <returns></returns>
		public static DateTime ToCustomerTime(this DateTime utcTime, TimeZoneInfo targetTimeZone)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(utcTime, targetTimeZone);
		}

		public static DateTime GetNextPermittedDate(this DateTime date, List<DayOfWeek> permittedDays, int getNext)
		{
			if (getNext == 0)
			{
				while (!permittedDays.Contains(date.DayOfWeek))
				{
					date = date.AddDays(1);
				}

				return date;
			}

			var sign = Math.Sign(getNext);
			var unsignedDays = Math.Abs(getNext);
			for (var i = 0; i < unsignedDays; i++)
			{
				do
				{
					date = date.AddDays(sign);
				} while (!permittedDays.Contains(date.DayOfWeek));
			}
			return date;
		}

		public static string ToPrice(this decimal price, string? currency = "en-US")
		{
			return price.ToString("c2", new System.Globalization.CultureInfo(currency));
		}

		public static string ToPrice(this decimal? price, string? currency = "en-US")
		{
			return ToPrice((price ?? 0), currency);
		}

		public static string CurrencySymbol(string? currency = "en-US")
		{
            CultureInfo cultureInfo = new CultureInfo(currency); // Replace with your desired culture (e.g., "en-IN")

            // Get the RegionInfo for the specified culture
            RegionInfo region = new RegionInfo(cultureInfo.Name);

            // Retrieve the currency symbol
            return region.CurrencySymbol;
        }
	}
}
