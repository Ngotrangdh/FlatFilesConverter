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

        public static List<string> SplitCSVLine(this string value, char delimiter)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<string>();
            }

            int length = value.Length;
            List<string> parts = new List<string>();
            bool isDoubleQuoteInLine = false;
            string part = string.Empty;

            for (int i = 0; i < length; i++)
            {
                char currentChar = value[i];

                if (currentChar == delimiter)
                {
                    if (!isDoubleQuoteInLine)
                    {
                        parts.Add(part);
                        part = string.Empty;
                        continue;
                    }
                }

                if (currentChar == '"')
                {
                    if (value[i - 1] == '"')
                    {
                        part += currentChar;
                    }
                    isDoubleQuoteInLine = !isDoubleQuoteInLine;
                    continue;
                }

                part += currentChar;

                if (i == length - 1)
                {
                    parts.Add(part);
                }
            }

            return parts;
        }
    }
}
