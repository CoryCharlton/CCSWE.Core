using System;

namespace CCSWE
{
    /// <summary>
    /// Contains extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Creates a new <see cref="DateTime"/> with the time set to 23:59:59.999.
        /// </summary>
        /// <param name="dateTime">The source <see cref="DateTime"/>.</param>
        /// <returns>A <see cref="DateTime"/> with the time set to 23:59:59.999.</returns>
        public static DateTime ToEndDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999, dateTime.Kind);
        }

        /// <summary>
        /// Creates a new <see cref="DateTime"/> with the time set to 00:00:00.000.
        /// </summary>
        /// <param name="dateTime">The source <see cref="DateTime"/>.</param>
        /// <returns>A <see cref="DateTime"/> with the time set to 00:00:00.000.</returns>
        public static DateTime ToStartDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0, dateTime.Kind);
        }
    }
}
