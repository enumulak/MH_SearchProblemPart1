namespace MH_SearchProblemPart1
{
    public sealed class PageStrength
    {
        //Private Variables
        private string pageName;
        private int strength;

        //Public Constructor with arguments - the only allowed to instantiate a PageStrength object
        public PageStrength(string page, int s)
        {
            pageName = page;
            pageName = " " + pageName.Trim();
            strength = s;
        }

        //Data Readers
        public string GetPageName() => pageName;
        public int GetPageStrength() => strength;
    }
}