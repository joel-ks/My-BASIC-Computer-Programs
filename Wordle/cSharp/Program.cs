using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordleCmdLine;

static class Program
{
    private const string WORD_LIST_FILE = "..\\wordlist";

    static void Main()
    {
        var words = LoadWords();
        Console.Error.WriteLine($"Loaded {words.Count} words");

        var rand = new Random();

        do
        {
            var game = new Game(ChooseRandomWord(words, rand));
            Console.WriteLine("-----"); // Imply the word has 5 letters
            game.Play();
        } while (PlayAgain());

        Console.WriteLine("Goodbye!");
    }

    private static List<string> LoadWords() => File.ReadAllLines(WORD_LIST_FILE).ToList();

    private static string ChooseRandomWord(IList<string> list, Random random) => list[random.Next(list.Count)];

    private static bool PlayAgain()
    {
        Console.Write("Play again? (Y/n) ");
        var key = Console.ReadKey(); // TODO: loop until Y/y, N/n, or Enter are pressed

        Console.WriteLine();

        return key.KeyChar != 'n' && key.KeyChar != 'N';
    }
}
