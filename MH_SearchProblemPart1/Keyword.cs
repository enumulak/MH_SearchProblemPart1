namespace MH_SearchProblemPart1
{
    public sealed class Keyword
    {
        //Private Variables
        private string theWord;
        private int weight;

        //Public Constructor with Arguments - the only one allowed for instantiating a Keyword object
        public Keyword(string word, int sIndex)
        {
            theWord = word;
            theWord = " " + theWord.Trim();
            weight = sIndex;
        }

        //Data Readers
        public string GetKeyword() => theWord;
        public int GetKeywordWeight() => weight;
    }
}