using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNostalgia
{
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

        public static string GetString(this IDataReader subject, string columnName)
        {
            var ordinal = subject.GetOrdinal(columnName);
            if (subject.IsDBNull(ordinal))
                return null;

            return GetValueWith(subject, subject.GetString, columnName);
        }

        public static int GetInt32(this IDataReader subject, string columnName)
        {
            return GetValueWith(subject, subject.GetInt32, columnName);
        }

        public static int? GetInt32Nullable(this IDataReader subject, string columnName)
        {
            return GetNullableValueWith<int>(subject, columnName);
        }

        public static bool GetBoolean(this IDataReader subject, string columnName)
        {
            return GetValueWith(subject, subject.GetBoolean, columnName);
        }

        public static bool? GetBooleanNullable(this IDataReader subject, string columnName)
        {
            return GetNullableValueWith<bool>(subject, columnName);
        }

        public static DateTime GetDateTime(this IDataReader subject, string columnName)
        {
            return GetValueWith(subject, subject.GetDateTime, columnName);
        }

        public static DateTime? GetDateTimeNullable(this IDataReader subject, string columnName)
        {
            return GetNullableValueWith<DateTime>(subject, columnName);
        }

        public static object GetValue(this IDataReader subject, string columnName)
        {
            var value = subject.GetValue(subject.GetOrdinal(columnName));

            if (DBNull.Value.Equals(value))
                return null;

            return value;
        }
    }
}
