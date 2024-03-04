using System.Reflection;

namespace TextProcessing.Tests
{
    public class BasicTests
    {
        [Fact]
        public void RemoveSpecialCharacters_ShouldRemoveSpecialCharacters()
        {
            // Arrange
            string entryTextPortuguese = "Esta é uma cadeia contendo caracteres especiais, como $ e @.";
            string expectedText = "Esta e uma cadeia contendo caracteres especiais como  e  .";
            Processing processor = new Processing();
            MethodInfo removeSpecialChars = typeof(Processing).GetMethod("RemoveSpecialChars", BindingFlags.NonPublic | BindingFlags.Instance);


            // Act

            Task<string> resultString = (Task<string>)removeSpecialChars.Invoke(processor, new object[]
            {
                entryTextPortuguese,
                true
            });

            // Assert
            Assert.Equal(expectedText, resultString.GetAwaiter().GetResult());
        }

        [Fact]
        public void TokenizeText_ShouldReturnTokenizedText()
        {
            // Arrange
            string rawText = "Texto de teste para checar o método";
            string[] expectedArray = new string[] { "Texto", "de", "teste", "para", "checar", "o", "método" };
            Processing processor = new Processing();
            MethodInfo tokenizeText = typeof(Processing).GetMethod("TokenizeText", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            string[] resultArray = (string[])tokenizeText.Invoke(processor, new object[]
            {
                rawText
            });

            // Assert
            Assert.Equal(expectedArray, resultArray);
        }

        [Theory]
        [InlineData("Esta cadeia de caracteres deve ser processada, stopwords e caracteres especiais como $ devem ser removidos.", "pt", false)]
        public async Task Process_ShouldReturnProcessedText(string text, string language, bool removeSpecialChars)
        {
            // Arrange
            Processing processor = new Processing();
            string expectedText = "cadeia caracteres deve ser processada stopwords caracteres especiais devem ser removidos.";

            // Act
            string result = await processor.Process(text, language, removeSpecialChars);

            // Assert
            Assert.Equal(expectedText, result);
            
        }
    }
}