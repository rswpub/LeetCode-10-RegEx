using LeetCode_10_RegEx;

namespace UnitTests
{
    public class MatchPattern
    {
        [Theory]
        [InlineData("aa", "a*")]
        [InlineData("ab", "..")]
        [InlineData("ab", "ab")]
        [InlineData("abab", "abab")]
        [InlineData("abab", "a.a.")]
        [InlineData("abab", "....")]
        [InlineData("abab", ".ba.")]
        [InlineData("abab", "a*b*..")]
        [InlineData("abab", "a*b*a*b*")]
        [InlineData("abbb", "a*b*")]
        [InlineData("abbb", "ab*")]
        [InlineData("abbb", ".b*")]
        [InlineData("abbb", ".b*b")]
        [InlineData("abbb", ".bb*")]
        [InlineData("abab", "ababz*x*y*.*r*")]
        public void PatternDoesMatch(string inputStr, string regExPattern)
        {
            // Arrange

            // Act
            bool actualResult = Program.IsMatch(inputStr, regExPattern);

            // Assert
            Assert.True(actualResult);
        }

        [Theory]
        [InlineData("aa", "a")]
        [InlineData("aa", "b")]
        [InlineData("aa", "b*")]
        [InlineData("aa", ".")]
        [InlineData("ab", ".*")]
        [InlineData("aab", "a.")]
        [InlineData("aab", "a.a")]
        [InlineData("aab", "a*")]
        public void PatternDoesNotMatch(string inputStr, string regExPattern)
        {
            // Arrange

            // Act
            bool actualResult = Program.IsMatch(inputStr, regExPattern);

            // Assert
            Assert.False(actualResult);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "")]
        [InlineData("a", " ")]
        [InlineData(" ", "a")]
        [InlineData("", ".")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaa", "a")]
        [InlineData("a", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [InlineData("A", ".")]
        [InlineData("a", "A")]
        [InlineData("1", "a")]
        [InlineData("a", "1")]
        [InlineData("a", "*")]
        [InlineData("a", "1*")]
        [InlineData("a", "**")]
        public void ThrowsExceptionWhenConditionsAreOutOfBounds(string inputStr, string regExPattern)
        {
            // Arrange

            // Act
            Action action = () => Program.IsMatch(inputStr, regExPattern);

            // Assert
            Exception exception = Assert.Throws<Exception>(action);

        }
    }
}