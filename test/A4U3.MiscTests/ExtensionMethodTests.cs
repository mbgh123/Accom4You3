using System;
using A4U3.Web.Helpers;
using Xunit;
using FluentAssertions;

namespace A4U3.MiscTests
{
    /// <summary>
    /// Testing the Shorten extension method.
    /// </summary>
    public class ExtensionMethodTests
    {
        [Fact]
        public void EmptyString_returns_EmptyString()
        {
            //Arrange
            string input = ""; // 0 chars

            //Act
            string result = input.Shorten(3, addDots: false);

            //Assert
            result.Should().Be(String.Empty, "When input is empty string, output should be empty string");
        }

        [Fact]
        public void ZeroLengthRequested_returns_EmptyString()
        {
            //Arrange
            string input = "one two three"; // some chars

            //Act
            string result = input.Shorten(0, addDots: false);

            //Assert
            result.Should().Be(String.Empty, "When ZERO length is requested, output should be empty string");
        }

        [Fact]
        public void LengthGreaterThanInput_NoChangeToInput()
        {
            //Arrange
            string input = "one two three"; // 13 chars

            //Act
            string result = input.Shorten(30, addDots: false);

            //Assert
            result.Should().Be(input, "When length exceeds input, output should match input");
        }

        [Fact]
        public void GoodInitialEndPoint_Space()
        {
            // The requested length is immediately on a good end point
            // no further shortening required

            //Arrange
            string input = "one two three"; // 13 chars

            //Act
            string result = input.Shorten(3, addDots:false);

            //Assert
            result.Should().Be("one");
        }

        [Fact]
        public void GoodInitialEndPoint_Period()
        {
            // The requested length is immediately on a good end point
            // no further shortening required

            //Arrange
            string input = "one. two three"; // 13 chars

            //Act
            string result = input.Shorten(3, addDots:false);

            //Assert
            result.Should().Be("one");
        }

        [Fact]
        public void AdjustmentRequired()
        {
            // The requested length is on a word
            // further shortening required

            //Arrange
            string input = "one two three"; // 13 chars

            //Act
            string result = input.Shorten(5, addDots:false);

            //Assert
            result.Should().Be("one");
        }

        [Fact]
        public void AdjustmentRequired2()
        {
            // The requested length is on a word
            // further shortening required

            //Arrange
            string input = "one. two three"; // 13 chars

            //Act
            string result = input.Shorten(6, addDots: false);

            //Assert
            result.Should().Be("one.");
        }
        [Fact]
        public void FirstWordHit()
        {
            // The requested length is on a the first word
            // further shortening required

            //Arrange
            string input = "one two three"; // 13 chars

            //Act
            string result = input.Shorten(2, addDots: false);

            //Assert
            result.Should().Be("o");
        }

        [Fact]
        public void AdjustmentRequired_WithDots()
        {
            // The requested length is on a word
            // further shortening required

            //Arrange
            string input = "one two three"; // 13 chars

            //Act
            string result = input.Shorten(5, addDots: true);

            //Assert
            result.Should().Be("one...");
        }
    }
}
