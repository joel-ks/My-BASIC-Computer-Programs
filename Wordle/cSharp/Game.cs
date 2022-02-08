using System;
using System.Collections.Generic;
using System.Linq;

namespace WordleCmdLine;

class Game
{
    private const int MAX_GUESSES = 6;
    private const string PROMPT = "> ";
    private static readonly char[][] KEYBOARD_CHARS = new char[][] {
        new char[] {'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'},
        new char[] {'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L'},
        new char[] {'Z', 'X', 'C', 'V', 'B', 'N', 'M'}
    };

    private string _word;
    private int _guesses = 0;
    private Dictionary<char, Clue> _letterClues = new Dictionary<char, Clue>();

    public Game(string word)
    {
        _word = word.ToUpper();
    }

    public void Play()
    {
        string guess;
        do
        {
            DrawKeyboard();
            Console.WriteLine();

            guess = ReadGuess().ToUpper();
            ++_guesses;
            GiveClues(guess);
        } while (guess != _word && _guesses < MAX_GUESSES);

        if (_guesses > MAX_GUESSES)
        {
            Console.WriteLine($"Sorry. You've used all your {MAX_GUESSES} guesses.");
            Console.WriteLine($"The word was '{_word}'");
        }
        else // guess == _word
        {
            Console.WriteLine("Well done!");
            Console.WriteLine($"You found the word '{_word}' in {_guesses}/{MAX_GUESSES} guesses");
        }
    }

    private string ReadGuess()
    {
        do
        {
            Console.Write(PROMPT);
            var guess = Console.ReadLine();

            if (guess != null && GuessIsValid(guess)) return guess;
            else Console.WriteLine("Please enter a 5-letter word."); // TODO: this could be nicer...
        } while (true);
    }

    private void GiveClues(string guess)
    {
        var clues = new Clue[5];
        for (var i = 0; i < _word.Length; ++i)
        {
            var letterI = guess[i];

            if (letterI == _word[i])
            {
                clues[i] = Clue.CorrectPosition;
                _letterClues[letterI] = Clue.CorrectPosition;
            }
            else if (_word.Contains(letterI))
            {
                clues[i] = Clue.InWord;

                // Don't add the clue to the dictionary if the correct position was already found
                _letterClues.TryAdd(letterI, Clue.InWord);
            }
            else
            {
                clues[i] = Clue.NotInWord;
                _letterClues[letterI] = Clue.NotInWord;
            }
        }

        for (int i=0; i < PROMPT.Length; ++i) Console.Write(' ');

        foreach (var clue in clues)
        {
            Console.BackgroundColor = clue.GetColour();
            Console.Write(clue.HintChar());
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    private void DrawKeyboard()
    {
        var maxRowLength = KEYBOARD_CHARS.Max(PaddedLength);

        foreach (var row in KEYBOARD_CHARS)
        {
            var padding = maxRowLength - PaddedLength(row);
            int padStart = (int)Math.Ceiling(padding / 2.0);

            Console.Write(string.Concat(Enumerable.Repeat(' ', padStart)));

            foreach (var c in row)
            {
                if (_letterClues.TryGetValue(c, out Clue clue))
                {
                    Console.BackgroundColor = clue.GetColour();
                }

                Console.Write(c);

                Console.ResetColor();
                Console.Write(' ');
            }

            Console.WriteLine();
        }
    }

    private bool GuessIsValid(string guess)
    {
        if (guess.Length != _word.Length) return false;
        
        if (!IsWord(guess)) return false;

        // TODO: check guess is in wordlist?

        return true;
    }

    private static int PaddedLength(char[] row) => row.Length * 2 - 1;

    private static bool IsWord(string str) => str.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));
}
