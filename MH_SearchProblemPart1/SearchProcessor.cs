using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_SearchProblemPart1
{
    public enum EntityType { Query, Page}

    public sealed class SearchProcessor
    {
        //Private Variables
        private List<WebEntity> queries = new List<WebEntity>();
        private List<WebEntity> pages = new List<WebEntity>();

        //Variables that impact Logic - All have values and are initialized at one place so that changes are easy to debug and maintain
        private readonly int maxEntities = 6;
        private readonly int maxKeywordStrength = 3;

        private SearchProcessor() { }

        //Static method to provide access to the Singleton instance of this Class
        public static SearchProcessor GetInstance { get; } = new SearchProcessor();

        public void Initiate()
        {
            //Start creating the required Web Entities

            //Methods that create Queries and Pages based on User Input - Commented out as we are currently using the Auto-Create Method below
            //queries = CreateEntities(EntityType.Query);
            //pages = CreateEntities(EntityType.Page);

            //Automatically generates Queries and Pages - Please comment out if the two above Methods are being used for User Input
            CreateEntitiesAuto();

            Console.WriteLine('\n');
            
            if(queries.Count > 0 && pages.Count > 0 )
            { DisplayInput(); }
            else
            { Console.WriteLine("There seems to be Insufficient Data for Processing.. Exiting Program..."); }

        }

        #region Private Methods

        
        private List<WebEntity> CreateEntities(EntityType e)
        {
            var eType = e.ToString();
            
            List<WebEntity> wEntities = new List<WebEntity>();
            List<string> keyWordList = new List<string>();

            Console.WriteLine("Now building {0} WebEntity(s) of type {1}...", maxEntities, eType);
            Console.WriteLine('\n');

            for(var i = 0; i < maxEntities; i++)
            {
                Console.WriteLine("How many Keywords for {0} {1} ?", eType, i + 1);
                var input = Console.ReadLine();

                if(int.TryParse(input, out int numberOfKeywords))
                {
                    for(var j = 0; j < numberOfKeywords; j++)
                    {
                        Console.WriteLine("Enter Keyword {0}: ", j + 1);
                        var kInput = Console.ReadLine();
                        keyWordList.Add(kInput);
                    }

                    var eName = eType.Substring(0, 1) + (i + 1);
                    
                    wEntities.Add(new WebEntity(eName, e, keyWordList, maxKeywordStrength));

                    keyWordList.Clear();
                }
                else
                {
                    Console.WriteLine("That seems to be an Invalid Input..");
                    Console.WriteLine('\n');
                    break;
                }
            }

            return wEntities;
        }

        private void CreateEntitiesAuto()
        {
            //Initialize a List of Queries
            queries = new List<WebEntity>
            {
                new WebEntity("Q1", EntityType.Query, new List<string>{"Ford"}, maxKeywordStrength),
                new WebEntity("Q2", EntityType.Query, new List<string>{"Car"}, maxKeywordStrength),
                new WebEntity("Q3", EntityType.Query, new List<string>{"Review"}, maxKeywordStrength),
                new WebEntity("Q4", EntityType.Query, new List<string>{"Ford", "Review"}, maxKeywordStrength),
                new WebEntity("Q5", EntityType.Query, new List<string>{"Ford", "Car"}, maxKeywordStrength),
                new WebEntity("Q6", EntityType.Query, new List<string>{"Cooking", "French"}, maxKeywordStrength)
            };

            //Initialize a List of Pages
            pages = new List<WebEntity>
            {
                new WebEntity("P1", EntityType.Page, new List<string>{"Ford", "Car", "Review"}, maxKeywordStrength),
                new WebEntity("P2", EntityType.Page, new List<string>{"Review", "Car"}, maxKeywordStrength),
                new WebEntity("P3", EntityType.Page, new List<string>{"Review", "Ford"}, maxKeywordStrength),
                new WebEntity("P4", EntityType.Page, new List<string>{"Toyota", "Car"}, maxKeywordStrength),
                new WebEntity("P5", EntityType.Page, new List<string>{"Honda", "Car"}, maxKeywordStrength),
                new WebEntity("P6", EntityType.Page, new List<string>{"Car"}, maxKeywordStrength)
            };
        }

        private void DisplayInput()
        {
            Console.WriteLine('\n');
            Console.WriteLine("******* SAMPLE INPUT *******");

            for(var i = 0; i < pages.Count; i++)
            { pages[i].DisplayAllKeywords(); }

            for(var k = 0; k < queries.Count; k++)
            { queries[k].DisplayAllKeywords(); }

            //Start Logic processing after Displaying the Input
            StartKeywordMatching();
        }

        private void StartKeywordMatching()
        {
            //We first loop through our Queries
            for(var i = 0; i < queries.Count; i++)
            {
                //Each Query is evaluated against All current Pages
                for(var k = 0; k < pages.Count; k++)
                {
                    MatchKeywords(queries[i], pages[k]);
                }
            }

            //If All goes Well, Display the Output
            DisplayOutput();
        }

        private void MatchKeywords(WebEntity query, WebEntity page)
        {
            //List that will store the Product of Query Keyword Weight and Page Keyword Weight
            List<int> productOfKeywordWeights = new List<int>();

            //Loop through each keyword of the given Query
            for(var i = 0; i < query.GetKeywordCount(); i++)
            {
                //Th current Keyword of the Query is stored in a local variable
                var currentKeyword = query.GetKeyword(i);

                //Now we loop through each Keyword of given Page
                for(var k = 0; k < page.GetKeywordCount(); k++)
                {
                    //We check if the current Query Keyword is equal to the Current Page Keyword. If so, then calculate the product of Keyword Weights and store that in the local list
                    if(currentKeyword.ToLower() == page.GetKeyword(k).ToLower())
                    {
                        var product = query.GetKeywordWeight(i) * page.GetKeywordWeight(k);
                        productOfKeywordWeights.Add(product);
                    }
                }
            }

            //Now ALL Keywords of the Query have been evaluated against ALL Keywords of the given Page 
            //If the Keyword Weight List count is more than 0, then the given Page is sent to the Query (Web Entity) class for further processing (The Query object decides how to proceed further)
            if(productOfKeywordWeights.Count > 0)
            { query.PreProcessPageStrength(page, productOfKeywordWeights); }
        }

        private void DisplayOutput()
        {
            Console.WriteLine('\n');
            Console.WriteLine("****** OUTPUT FOR THE SAMPLE INPUT *******");
            for(var i = 0; i < queries.Count; i++)
            {
                queries[i].DisplayPageStrengths();
            }
        }

        #endregion
    }
}
