using System;

namespace Demo7
{
    public class LevelCreator
    {
        List<MarkovColumn> markovColumns;
        LevelParser levelParser;
        public LevelCreator()
        {
            markovColumns = new List<MarkovColumn>();
            levelParser = new LevelParser();
        }

        public void Parse(string path)
        {
            levelParser.Parse(path);
        }

        // Generate level of given length as Markov columns
        public void Generate(int levelLength)
        {
            // Length zero exception
            if (levelLength == 0) { throw new ArgumentException("Level length can't be zero."); }

            // Generate initial column (more frequent = more likely)
            int initialColIndex = Program.random.Next(0, levelParser.getColumns().Count);
            string initialColumn = levelParser.getColumns()[initialColIndex];
            MarkovColumn? initialMarkovColumn = levelParser.getMarkovColumns().Find(x => x.column == initialColumn);
            if (initialMarkovColumn == null) { throw new Exception("Column found is not in Markov Columns."); }
            markovColumns.Add(initialMarkovColumn);

            // Generate rest of level
            for (int i = 0; i < levelLength - 1; i++)
            {
                // Select last column
                MarkovColumn currMarkovColumn = markovColumns[markovColumns.Count - 1];
                // Add new column
                string nextColumn = currMarkovColumn.GetNext();
                MarkovColumn? nextMarkovColumn = levelParser.getMarkovColumns().Find(x => x.column == nextColumn);
                if (nextMarkovColumn == null) { throw new Exception("Column found is not in Markov Columns."); }
                markovColumns.Add(nextMarkovColumn);
            }
        }

        // Generate file with given name
        public void ToFile(string path)
        {
            File.WriteAllText(path, GetLevelString());
        }

        // Get properly formatted level
        public string GetLevelString()
        {
            string rowsString = "";
            bool firstLine = true;

            int levelLength = markovColumns[0].column.Length;
            for (int i = 0; i < levelLength; i++)
            {
                String currString = "";
                foreach (MarkovColumn mcol in markovColumns)
                {
                    currString += mcol.column[i];
                }
                // Prevents empty newline on the first line
                if (firstLine) { rowsString += currString; firstLine = false; }
                else { rowsString += "\n" + currString; }
            }
            return rowsString;
        }

        public void Restart()
        {
            markovColumns = new List<MarkovColumn>();
            levelParser = new LevelParser();
        }
    }
}
