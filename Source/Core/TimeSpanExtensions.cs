using System;

namespace CCSWE
{
    /// <summary>
    /// Contains extension methods for <see cref="TimeSpan"/>.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to a formatted string.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> to convert.</param>
        /// <param name="showMilliseconds">Whether or not to display milliseconds.</param>
        /// <returns>A formatted string.</returns>
        public static string ToFriendlyString(this TimeSpan timeSpan, bool showMilliseconds = false)
        {
            string returnValue;

            if (timeSpan.TotalHours >= 24)
            {
                returnValue = $"{timeSpan.Days}.{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            }
            else if (timeSpan.TotalHours >= 1)
            {
                returnValue = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            }
            else
            {
                returnValue = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            }

            if (showMilliseconds)
            {
                returnValue += $".{timeSpan.Milliseconds:000}";
            }

            return returnValue;
        }
    }
}
