using System;
using System.Collections.Generic;
using koninkrijk.Server.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace koninkrijk.Tests.Helpers
{
    public class WordGenTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WordGenTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void Words_Are_Of_Correct_Length(int length)
        {
            // Arrange
            var words = WordGen.WordsByLength;
            var errors = new List<string>();

            // Act
            foreach (var word in words[length])
            {
                try
                {
                    Assert.Equal(length, word.Length);
                }
                catch (Exception ex)
                {
                    errors.Add($"Faulty word of length {length}: {word} - Exception: {ex.Message}");
                }
            }

            // Assert
            if (errors.Count > 0)
            {
                var errorMessage = string.Join("\n", errors);
                _testOutputHelper.WriteLine(errorMessage);
                throw new Xunit.Sdk.XunitException(errorMessage);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void Words_Are_In_Dutch_Dictionary(int length)
        {
            // Arrange
            var words = WordGen.WordsByLength;
            var errors = new List<string>();


            // Act
            foreach (var word in words[length])
            {
                bool validWord = SpellCheck.checkWord(word);
                try
                {
                    Assert.True(validWord);
                }
                catch (Exception ex)
                {
                    errors.Add($"Word {word} is not in the dictionary file- Exception: {ex.Message}");
                }
            }

            // Assert
            if (errors.Count > 0)
            {
                var errorMessage = string.Join("\n", errors);
                _testOutputHelper.WriteLine(errorMessage);
                throw new Xunit.Sdk.XunitException(errorMessage);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public void Words_Are_Unique(int length)
        {
            // Arrange
            var words = WordGen.WordsByLength;
            var errors = new List<string>();

            // Act
            try
            {
                var duplicates = words[length].GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                if (duplicates.Any())
                {
                    errors.Add($"Dictionary for {length} has duplicates: {string.Join(", ", duplicates)}");
                }
                Assert.True(!duplicates.Any());
            }
            catch (Exception ex)
            {
                errors.Add($"Exception for length {length} - Exception: {ex.Message}");
            }

            // Assert
            if (errors.Count > 0)
            {
                var errorMessage = string.Join("\n", errors);
                _testOutputHelper.WriteLine(errorMessage);
                throw new Xunit.Sdk.XunitException(errorMessage);
            }
        }


        //[Fact]
        //public void GenerateWord_InvalidLength_ThrowsInvalidOperationException()
        //{
        //    // Arrange
        //    var invalidLength = 10;
        //    var playerGuessSeed = 42;

        //    // Act & Assert
        //    Assert.Throws<InvalidOperationException>(() => WordGen.GenerateWord(invalidLength, playerGuessSeed));
        //}

        //[Fact]
        //public void GenerateWord_DifferentSeeds_ReturnDifferentWords()
        //{
        //    // Arrange
        //    var length = 5;
        //    var playerGuessSeed1 = 42;
        //    var playerGuessSeed2 = 25;

        //    // Act
        //    var result1 = WordGen.GenerateWord(length, playerGuessSeed1);
        //    var result2 = WordGen.GenerateWord(length, playerGuessSeed2);

        //    // Assert
        //    Assert.NotEqual(result1, result2);
        //}
    }
}
