using System;

namespace Demo7
{
    public class MarkovColumn
    {
        public string column;
        public int frequency;
        public List<(string, int)> transitions; // Following nodes (and their frequency after me)

        public MarkovColumn(string column, int frequency)
        {
            this.column = column;
            this.frequency = frequency;
            transitions = new List<(string, int)>();
        }

        // Choose next column according to transition frequency
        public string GetNext()
        {
            // Get total number of transitions (including repetitions)
            int totalTransitionFrequency = 0;
            foreach (var transition in transitions)
            {
                totalTransitionFrequency += transition.Item2;
            }

            // Random number in range [0, totalTransitionFrequency] as objective
            int objective = Program.random.Next(totalTransitionFrequency+1);

            // Choose column to return
            int sum = 0;
            foreach (var transition in transitions)
            {
                sum += transition.Item2;
                // Return if objective is reached or surpassed
                if (sum >= objective)
                {
                    return transition.Item1;
                }
            }
            // throw error if nothing has been returned yet
            throw new Exception("No transition was returned. Check function.");
        }


    }
}
