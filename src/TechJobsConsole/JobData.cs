using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        //The JobData class is responsible for importing the data from the CSV file and
        //parsing it into a C#-friendly format; that is, into Dictionary and List form.
        //The LoadData method, in particular, creates the AllJobs property of type List<Dictionary<string, string>>

        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            LoadData();
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            foreach (Dictionary<string, string> job in AllJobs)
            {
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    values.Add(aValue);
                }
            }
            return values;
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                string aValue = row[column];
                string aValueLower = aValue.ToLower();

                if (aValueLower.Contains(value.ToLower()))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }

        public static string[] getColumnLabelsFromScratch()
        {
            string[] firstRow;
            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                string line = reader.ReadLine();
                string[] rowArray = CSVRowToStringArray(line);
                return rowArray;
            }
        }


        static Boolean SpecificFieldContainsStringMatch(Dictionary<string, string> dict, string s, string fieldKey)
        {
            string field = dict[fieldKey];
            if (field.ToLower().Contains(s.ToLower()))
            {
                return true;
            }

            return false;
        }

        public static List<Dictionary<string, string>>
            jobsMatchingTermInAnyField(string s, string fieldKey)
        {
            List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
            Boolean printThisOne;
            List<Dictionary<string, string>> AllTheJobs = JobData.FindAll();
            foreach (Dictionary<string, string> dict in AllTheJobs)
            {
                printThisOne = SpecificFieldContainsStringMatch(dict, s, fieldKey);

                if (printThisOne)
                {
                    ret.Add(dict);
                }
            }
            return ret;
        }

        static Boolean anyDictValueContainsStringMatch(Dictionary<string, string> dict, string searchTerm)
        {
            foreach (string field in dict.Values)
            {
                if (field.ToLower().Contains(searchTerm.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Dictionary<string, string>> FindByValue(string searchTerm)
        {
            List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
            Boolean printThisOne;
            List<Dictionary<string, string>> AllTheJobs = JobData.FindAll();
            foreach (Dictionary<string, string> dict in AllTheJobs)
            {
                printThisOne = anyDictValueContainsStringMatch(dict, searchTerm);

                if (printThisOne)
                {
                    ret.Add(dict);
                }
            }
            return ret;
        }

    }
}
