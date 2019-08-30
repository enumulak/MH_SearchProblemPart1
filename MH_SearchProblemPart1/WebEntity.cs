using System;
using System.Collections.Generic;
using System.Linq;

namespace MH_SearchProblemPart1
{
    public class WebEntity
    {
        //Private Variables
        private readonly string Name;
        private readonly string typeOfEntity;
        private readonly List<Keyword> keywords = new List<Keyword>();
        private readonly int maxKeywordWeight;

        //List to hold objects of PageStrength Type - Will be used and applicable only to the QUERY type of Entity
        private List<PageStrength> pageStrengths = new List<PageStrength>();

        //Public Constructor with arguments - the only one allowed for instantiating a Web Entity
        public WebEntity(string name, Enum entityType, List<string> theKeywords, int strength)
        {
            Name = name;
            typeOfEntity = entityType.ToString();
            maxKeywordWeight = strength;

            if(theKeywords.Count() > 0)
            {
                for(var i = 0; i < theKeywords.Count(); i++)
                {
                    keywords.Add(new Keyword(theKeywords[i], maxKeywordWeight));
                    maxKeywordWeight--;
                }
            }
            else
            {
                Console.WriteLine("Insufficient Data...Exiting Program...");
            }
        }

        #region Public Methods

        //Method used by the current Web Entity to display all its Keywords
        public void DisplayAllKeywords()
        {
            var combined = "";

            if(keywords.Count > 0)
            {
                for(var i = 0; i < keywords.Count; i++)
                {
                    combined = string.Concat(keywords.Select(o => o.GetKeyword()));
                }

                Console.WriteLine("{0} : {1}", GetEntityName(), combined);
            }
            else
            {
                Console.WriteLine("{0} has no Keywords!", GetEntityName());
            }
        }

        public void PreProcessPageStrength(WebEntity page, List<int> keywordWeights)
        {
            if(page != null && keywordWeights != null)
            { ProcessPageStrength(page, keywordWeights); }
            else
            { Console.WriteLine("There was a problem.. Please check the Supplied Data.."); }
        }

        //Method used mainly by the QUERY type to display a List of Relevant Pages and their strengths
        public void DisplayPageStrengths()
        {
            var combined = "";

            List<PageStrength> fivePageList = new List<PageStrength>();

            if(typeOfEntity.ToLower() == "query")
            {
                if (pageStrengths.Count > 5)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        fivePageList.Add(pageStrengths[i]);
                    }

                    combined = string.Concat(fivePageList.Select(o => o.GetPageName()));

                    Console.WriteLine("{0} : {1}", GetEntityName(), combined);

                }
                else
                {
                    for (var i = 0; i < pageStrengths.Count; i++)
                    {
                        combined = string.Concat(pageStrengths.Select(o => o.GetPageName()));
                    }

                    Console.WriteLine("{0} : {1}", GetEntityName(), combined);
                }
            }
            else
            {
                Console.WriteLine("There is nothing to display for the given Entity...");
            }
        }

        #endregion

        #region Private Methods

        //Method mainly used by the QUERY type to Process Strength of a Page
        private void ProcessPageStrength(WebEntity page, List<int> keywordWeights)
        {
            //The Sum of All Keyword Weights is calculated and stored
            var sum = keywordWeights.Sum();

            //Page Strength Object is created with the incoming PAGE and the Sum of Keyword Weights. This object is then stored in the private list member
            pageStrengths.Add(new PageStrength(page.GetEntityName(), sum));

            //The Pagestrength List is then Sorted, in descending order, according to the Strength Value
            pageStrengths.Sort((x, y) => y.GetPageStrength().CompareTo(x.GetPageStrength()));
        }

        #endregion

        #region Data Readers

        public string GetEntityName() => Name;
        public string GetEntityType() => typeOfEntity;
        public string GetKeyword(int index) => keywords[index].GetKeyword();
        public int GetKeywordWeight(int index) => keywords[index].GetKeywordWeight();
        public int GetKeywordCount() => keywords.Count;

        #endregion
    }
}
