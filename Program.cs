using System;
using System.IO;

using Demo7;

class Program
{
    public static Random random = new Random();

    static int Main(string[] args)
    {
        string path = "mario-1-1.txt";
        LevelCreator levelCreator = new LevelCreator();
        levelCreator.Parse(path);
        levelCreator.Generate(202);
        levelCreator.ToFile("generatedLevel.txt");
        Console.WriteLine(levelCreator.GetLevelString());
        levelCreator.Restart();

        return 1;
    }
}
