﻿using System.Linq;

class Program
{
    public static void Main(string[] args)
    {

        string FireContent = File.ReadAllText("data.txt");
        List<string> WordsToIgnore = new List<string>()
        {
            "a", "is", "of", "as", "are", "to", "in", "and", "by", "may", "be", ""
        };

        FireContent = FireContent.ToLower()
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(".", "");
       

        Dictionary<string, int> Occurances = new Dictionary<string, int>();

        List<string> Words = FireContent.Split(" ")
                    .Where(word => !WordsToIgnore.Contains(word.ToLower())).ToList();

        Console.WriteLine(FireContent.Split(" ").Count());

        foreach (var word in Words)
        {
            if (Occurances.ContainsKey(word.ToLower()))
            {
                int count = Occurances[word.ToLower()] + 1;
                Occurances[word.ToLower()] = count;
            }
            else
            {
                Occurances.Add(word.ToLower(), 1);
            }
        }

        Console.WriteLine("Words" + "\t\t" + "Occurances");
        Console.WriteLine("-------------------------------");

        foreach (string key in Occurances.Keys)
            Console.WriteLine("{0} \t\t {1}", key, Occurances[key]);

        Console.WriteLine("Total words count: " + Occurances.Count);


    }

}

