using System;

namespace Demo7
{
    public class LevelParser
    {
        List<string> columns;
        List<string> rows;
        int totalUniqueColumns;
        List<MarkovColumn> markovColumns;

        public LevelParser()
        {
            columns = new List<string>();
            rows = new List<string>();
            markovColumns = new List<MarkovColumn>();
            totalUniqueColumns = 0;
        }

        public List<string> getColumns() { return columns; }
        public List<MarkovColumn> getMarkovColumns() { return markovColumns; }

        // Rows and columns parsing. Instantiation of MarkovColumns.
        public void Parse(string path)
        {
            // Parse row strings
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                rows.Add(line);
                //Console.WriteLine(line);
            }

            // Parse column strings
            int rowLength = rows[0].Length;
            for (int i = 0; i < rowLength; i++)
            {
                String currString = "";
                foreach (String row in rows)
                {
                    currString += row[i];
                }
                columns.Add(currString);
                //Console.WriteLine(currString);
            }

            // Find unique columns and frequency
            List<(string, int)> currentColumnsAndRepetitions = new List<(string, int)>();

            foreach (string column in columns)
            {
                int currIndex = currentColumnsAndRepetitions.FindIndex(col => col.Item1 == column);
                // If repeated column is found, new repetition is added
                if (currIndex != -1)
                {
                    (string, int) aux = currentColumnsAndRepetitions[currIndex];
                    aux.Item2++;
                    currentColumnsAndRepetitions[currIndex] = aux;
                }
                // Otherwise, it's a new column
                else
                {
                    currentColumnsAndRepetitions.Add(new(column, 1));
                    //Console.WriteLine(column);
                }
            }
            //foreach (var tup in currentColumnsAndRepetitions) { Console.WriteLine(tup); }

            // Create MarkovColumn instances with associated string and frequency.
            totalUniqueColumns = currentColumnsAndRepetitions.Count;
            foreach ((string, int) uniqueTup in currentColumnsAndRepetitions)
            {
                MarkovColumn newMarkovColumn = new MarkovColumn(uniqueTup.Item1, uniqueTup.Item2);
                markovColumns.Add(newMarkovColumn);
            }

            // Parse transition probability for each unique markov column
            _ParseTransitions();

            //DEBUG
            //Console.WriteLine(markovColumns[0].getNext());
        }

        // Parse transition frequency for each unique markov column
        void _ParseTransitions()
        {
            foreach(MarkovColumn mcol in markovColumns)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    // If I found myself and I'm not the last column, check next one
                    if (mcol.column == columns[i] && i < columns.Count-1)
                    {
                        // If repeated next-column, add one to their frequency regarding me
                        int currNextIndex = mcol.transitions.FindIndex(x => x.Item1 == columns[i+1]);
                        if (currNextIndex != -1)
                        {
                            (string, int) aux = mcol.transitions[currNextIndex];
                            aux.Item2++;
                            mcol.transitions[currNextIndex] = aux;
                        }
                        // if new next-column, add new transition 
                        else
                        {
                            (string, int) newTransition = (columns[i+1], 1);
                            mcol.transitions.Add(newTransition);
                        }
                    }
                }
            }
        }
    }
}
