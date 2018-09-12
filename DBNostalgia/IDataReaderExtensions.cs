using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
    /// <summary>
    /// Visitor/Method-Extension class for all the GetValue methods for IDataReaders.
    /// </summary>
    public static class IDataReaderExtensions
    {
        private static T GetValueWith<T>(IDataReader subject, Func<int, T> getClosure, string columnName)
        {
            var ordinal = subject.GetOrdinal(columnName);
            var value = getClosure(ordinal);
            return value;
        }

        private static T? GetNullableValueWith<T>(IDataReader subject, string columnName)
            where T : struct
        {
            var ordinal = subject.GetOrdinal(columnName);
            var value = subject.GetValue(ordinal);

            if (DBNull.Value.Equals(value))
                return null;

            return (T)value;
        }

        /// <summary>
        /// Fetch a byte array from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>A byte[] array object</returns>
        public static byte[] GetBytes(this IDataReader subject, string columnName)
        {
            var ordinal = subject.GetOrdinal(columnName);
            if (subject.IsDBNull(ordinal))
                return default(byte[]);

            long dataLength = subject.GetBytes(ordinal, 0, null, 0, 0);
            var bytes = new byte[dataLength];
            var bufferSize = 1024;
            var bytesRead = 0L;
            var curPos = 0;
            while (bytesRead < dataLength)
            {
                bytesRead += subject.GetBytes(ordinal, curPos, bytes, curPos, bufferSize);
                curPos += bufferSize;
            }

            return bytes;
        }

        /// <summary>
        /// Fetch a string from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>A string value</returns>
        public static string GetString(this IDataReader subject, string columnName)
        {
            var ordinal = subject.GetOrdinal(columnName);
            if (subject.IsDBNull(ordinal))
                return null;

            return GetValueWith(subject, subject.GetString, columnName);
        }

        /// <summary>
        /// Fetch an integer from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>An integer value</returns>
        public static int GetInt32(this IDataReader subject, string columnName)
        {
            return GetValueWith(subject, subject.GetInt32, columnName);
        }

        /// <summary>
        /// Fetch a nullable integer from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>An integer value that is null when the database value is null</returns>
        public static int? GetInt32Nullable(this IDataReader subject, string columnName)
        {
            return GetNullableValueWith<int>(subject, columnName);
        }

        /// <summary>
        /// Fetch a boolean from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>A boolean value</returns>
        public static bool GetBoolean(this IDataReader subject, string columnName)
        {
            return GetValueWith(subject, subject.GetBoolean, columnName);
        }

        /// <summary>
        /// Fetch a nullable boolean from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>A boolean value that is null when the database value is null</returns>
        public static bool? GetBooleanNullable(this IDataReader subject, string columnName)
        {
            return GetNullableValueWith<bool>(subject, columnName);
        }

        /// <summary>
        /// Fetch a datetime from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>A datetime value</returns>
        public static DateTime GetDateTime(this IDataReader subject, string columnName)
        {
            return GetValueWith(subject, subject.GetDateTime, columnName);
        }

        /// <summary>
        /// Fetch a datetime from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>A datetime value that is null when the database value is null</returns>
        public static DateTime? GetDateTimeNullable(this IDataReader subject, string columnName)
        {
            return GetNullableValueWith<DateTime>(subject, columnName);
        }

        /// <summary>
        /// Fetch just the value as object from a column.
        /// </summary>
        /// <param name="subject">IDataReader visitee</param>
        /// <param name="columnName">Column to fetch from</param>
        /// <returns>An object that contains the value of the database, but its null when the database value is null, instead of DBNull</returns>
        public static object GetValue(this IDataReader subject, string columnName)
        {
            var value = subject.GetValue(subject.GetOrdinal(columnName));

            if (DBNull.Value.Equals(value))
                return null;

            return value;
        }
    }
}
