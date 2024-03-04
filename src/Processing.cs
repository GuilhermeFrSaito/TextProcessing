using System.Text;
using StopWord;

namespace TextProcessing
{
    public class Processing
    {
        public async Task<string> Process(string text, string language, bool removeSpecialChars)
        {
            string rawText = text.ToLower();

            string cleanText = await RemoveSpecialChars(rawText, removeSpecialChars);

            // TODO: implementar de forma segura a remoção de stopwords
            // rawString = rawString.RemoveStopWords(language);

            if (language == "pt")
                cleanText = cleanText.RemoveStopWords("pt");
            else if (language == "en")
                cleanText = cleanText.RemoveStopWords("en");

            string[] tokenizedWords = TokenizeText(cleanText);

            return await Task.Run(() => string.Join(" ", tokenizedWords));
        }

        private async Task<string> RemoveSpecialChars(string text, bool removeSpecialChars)
        {
            Dictionary<char, char> specialCharMap = new()
            {
                { 'á', 'a' }, { 'à', 'a' }, { 'ã', 'a' }, { 'â', 'a' },{ 'ä', 'a' },
                { 'é', 'e' }, { 'è', 'e' }, { 'ê', 'e' }, { 'ë', 'e' },
                { 'í', 'i' }, { 'ì', 'i' }, { 'î', 'i' }, { 'ï', 'i' },
                { 'ó', 'o' }, { 'ò', 'o' }, { 'õ', 'o' }, { 'ö', 'o' },
                { 'ú', 'u' }, { 'ù', 'u' }, { 'û', 'u' }, { 'ü', 'u' },
                { 'ñ', 'n' }, { 'ç', 'c' },
                
            };

            Dictionary<char, char> symbolCharMap = new()
            {
                { ',', ' ' }, { ';', ' ' }, { '/', ' ' }, { '\\', ' ' },
                { '$', ' ' }, { '@', ' ' }, { '!', ' ' }, { '?', ' ' },
                { ':', ' ' }, { '+', ' ' }, { '(', ' ' }, { ')', ' ' }, { '[', ' ' },
                { ']', ' ' }, { '{', ' ' }, { '}', ' ' }, { '&', ' ' }, { '%', ' ' },
                { '*', ' ' }, { 'º', ' ' }, { 'ª', ' ' }, { '|', ' ' }, { '\n', ' ' }
            };

            if (removeSpecialChars)
                symbolCharMap = specialCharMap.Union(symbolCharMap).ToDictionary<char, char>();

            StringBuilder builder = new();
            char? lastAddedChar = null;

            char[] inputArray = text.ToCharArray();

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (lastAddedChar != null && lastAddedChar == ' ' && inputArray[i] == ' ')
                    continue;

                if (symbolCharMap.TryGetValue(inputArray[i], out char value))
                {
                    builder.Append(value);
                    lastAddedChar = value;
                }
                else
                {
                    builder.Append(inputArray[i]);
                    lastAddedChar = inputArray[i];
                }
                    
            }

            return await Task.Run(() => builder.ToString());
        }

        private string[] TokenizeText(string text)
        {
            return text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
    }

    
}
