using FlatFilesConverter.Core.Utililies;
using Xunit;

namespace FlatFilesConverter.Core.Tests
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("abc", 1, 1, "b")]
        [InlineData("abc", 1, 2, "bc")]
        [InlineData("abc", 1, 5, "bc")]
        [InlineData("abc", 1, 0, "")]
        [InlineData("abc", 0, 3, "abc")]
        public void SubStr_ReturnExpectedSubString(string originalString, int startIndex, int length, string expectedResult)
        {
            Assert.Equal(expectedResult, originalString.SubStr(startIndex, length));
        }
    }
}
