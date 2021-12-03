using System;
using System.Collections.Generic;
using System.IO;

namespace TechJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            // Create two Dictionary vars to hold info for menu and data

            // Top-level menu options
            Dictionary<string, string> actionChoices = new Dictionary<string, string>();
            actionChoices.Add("search", "Search");
            actionChoices.Add("list", "List");

            // Column options
            Dictionary<string, string> columnChoices = new Dictionary<string, string>();
            columnChoices.Add("core competency", "Skill"); //Key ("core competency") is internal string we use to refer to option
            //Value ("Skill") is external way options will be shown to user.
            columnChoices.Add("employer", "Employer");
            columnChoices.Add("location", "Location");
            columnChoices.Add("position type", "Position Type");
            columnChoices.Add("all", "All");

            Console.WriteLine("Welcome to LaunchCode's TechJobs App!");


            void printStringArray(string[] yourArray)
            {
                foreach (string item in yourArray)
                {
                    Console.WriteLine(item.ToString());
                }
            }

            //printStringArray(JobData.getColumnLabelsFromScratch());

            // Allow user to search/list until they manually quit with ctrl+c
            while (true)
            {
                //Allow user to choose how to view data: list or search
                string actionChoice = GetUserSelection("View Jobs", actionChoices);

                if (actionChoice.Equals("list"))
                {
                    //Let user choose which column to list
                    string columnChoice = GetUserSelection("List", columnChoices);

                    if (columnChoice.Equals("all"))
                    {
                        PrintJobs(JobData.FindAll());
                    }
                    else
                    {
                        List<string> results = JobData.FindAll(columnChoice);

                        Console.WriteLine("\n*** All " + columnChoices[columnChoice] + " Values ***");
                        foreach (string item in results)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
                else // choice is "search"
                {
                    // How does the user want to search (e.g. by skill or employer)
                    string columnChoice = GetUserSelection("Search", columnChoices);

                    // What is their search term?
                    Console.WriteLine("\nSearch term: ");
                    string searchTerm = Console.ReadLine();

                    List<Dictionary<string, string>> searchResults;

                    // Fetch results
                    if (columnChoice.Equals("all"))
                    {
                        //TODO: Add search all fields functionality
                        searchResults = JobData.FindByValue(searchTerm);
                        PrintJobs(searchResults);
                    }
                    else
                    {
                        searchResults = JobData.FindByColumnAndValue(columnChoice, searchTerm);
                        PrintJobs(searchResults);
                    }
                }
            }
        }

        /*
         * Returns the key of the selected item from the choices Dictionary
         */
        private static string GetUserSelection(string choiceHeader, Dictionary<string, string> choices)
        {
            int choiceIdx;
            bool isValidChoice = false;
            string[] choiceKeys = new string[choices.Count];

            int i = 0;
            foreach (KeyValuePair<string, string> choice in choices)
            {
                choiceKeys[i] = choice.Key;
                i++;
            }

            do
            {
                Console.WriteLine("\n" + choiceHeader + " by:");

                for (int j = 0; j < choiceKeys.Length; j++)
                {
                    Console.WriteLine(j + " - " + choices[choiceKeys[j]]);
                }

                string input = Console.ReadLine();
                choiceIdx = int.Parse(input);

                if (choiceIdx < 0 || choiceIdx >= choiceKeys.Length)
                {
                    Console.WriteLine("Invalid choices. Try again.");
                    //If chosen index is invalid, the do-while loop
                    //will re-prompt the user for it.
                }
                else
                {
                    isValidChoice = true;
                }

            } while (!isValidChoice);

            return choiceKeys[choiceIdx];
        }

        static void printDictList(List<Dictionary<string, string>> listToPrint)
        {
            if (listToPrint.Count == 0)
            {
                Console.WriteLine("No results");
                return;
            }
            Console.WriteLine("*****");
            foreach (Dictionary<string, string> dict in listToPrint)
            {
                foreach (string s in dict.Keys)
                {
                    Console.WriteLine($"{s}: {dict[s]}");
                }
                Console.WriteLine("*****");
            }
        }

        private static void PrintJobs(List<Dictionary<string, string>> someJobs)
        {
            //TODO: Implement PrintJobs
            //List<Dictionary<string, string>> allJobs = JobData.FindAll();
            printDictList(someJobs);
        }



        void searchFieldWithTerm(string searchTerm, string fieldKey)
        {
            Console.WriteLine($"Searching field ${fieldKey} for term '{searchTerm}'");
            printDictList(JobData.jobsMatchingTermInAnyField(searchTerm, fieldKey));
        }



    }
}
