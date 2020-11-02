using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatFilesConverter.Core.Utililies
{
    public static class StringExtensions
    {
        public static string SubStr(this string value, int startIndex, int length)
        {
            int Length = value.Length;

            if (startIndex > Length - length)
            {
                return value.Substring(startIndex, Length - startIndex);
            }

            return value.Substring(startIndex, length);
        }
    }
}
