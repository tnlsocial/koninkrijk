using System.Diagnostics;
using System.IO;
using WeCantSpell.Hunspell;

namespace koninkrijk.Server.Helpers
{
    // Need to make this DI sometime for testability
    public static class SpellCheck
    {
        public static bool checkWord(string word)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string affixFile = Path.Combine(workingDirectory, "nl.aff"); 
            string dictionaryFile = Path.Combine(workingDirectory, "nl.dic");

            using var dictionaryStream = File.OpenRead(dictionaryFile);
            using var affixStream = File.OpenRead(affixFile);

            var dictionary = WordList.CreateFromStreams(dictionaryStream, affixStream);

            bool check = dictionary.Check(word);

            return check;
        }
    }
}
