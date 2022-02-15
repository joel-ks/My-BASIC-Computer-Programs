using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordleCmdLine;

static class Program
{
    private const string WORD_LIST_FILE = "..\\wordlist";
    private const string GUESS_LIST_FILE = "..\\guesslist";

    static void Main()
    {
        Console.WriteLine("***** Command line Wordle *****");
        Console.WriteLine("Based on the original at https://www.powerlanguage.co.uk/wordle/");
        Console.WriteLine("This recreation ©2022 Joel Kusasira-Sutton");
        Console.WriteLine();

        Console.WriteLine("Loading word list...");
        var words = LoadWords(WORD_LIST_FILE);
        var allowedGuesses = LoadWords(GUESS_LIST_FILE);

        Console.WriteLine("Ready.");
        Console.WriteLine();

        // TODO: option to show help text somehow?

        var rand = new Random();

        do
        {
            var game = new Game(ChooseRandomWord(words, rand), allowedGuesses);
            game.Play();
        } while (PlayAgain());

        Console.WriteLine("Goodbye!");
    }

    private static List<string> LoadWords(string filename) => File.ReadAllLines(filename).Select(s => s.ToUpper()).ToList();

    private static string ChooseRandomWord(IList<string> list, Random random) => list[random.Next(list.Count)];

    private static bool PlayAgain()
    {
        Console.Write("Play again? (Y/n) ");

        do
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Y || key.Key == ConsoleKey.N || key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine(key.KeyChar);
                return key.Key != ConsoleKey.N;
            }
        } while (true);
    }
}
