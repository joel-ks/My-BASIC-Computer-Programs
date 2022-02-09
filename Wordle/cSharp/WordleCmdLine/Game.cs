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
    private List<string> _wordlist;
    private Dictionary<char, int> _letterFreqs;
    private int _guesses = 0;
    private Dictionary<char, Clue> _letterClues = new Dictionary<char, Clue>();

    public Game(string word, IEnumerable<string> wordlist)
    {
        _word = word.ToUpper();
        _wordlist = new List<string>(wordlist);
        _wordlist.Sort();
        _letterFreqs = _word.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
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

            var clues = GetClues(guess);
            GiveClues(clues);
            UpdateLetterClues(clues);
        } while (guess != _word && _guesses < MAX_GUESSES);

        if (guess == _word)
        {
            Console.WriteLine("Well done!");
            Console.WriteLine($"You found the word '{_word}' in {_guesses}/{MAX_GUESSES} guesses");
        }
        else // ran out of guesses
        {
            Console.WriteLine($"Sorry. You've used all your {MAX_GUESSES} guesses.");
            Console.WriteLine($"The word was '{_word}'");
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

    private WordWithClues GetClues(string guess)
    {
        var markedLetterFreqs = new Dictionary<char, int>();
        var clues = new WordWithClues(_word.Length);

        // First pass - check for CorrectPosition letters as these should always be marked regardless of position
        for (var i = 0; i < _word.Length; ++i)
        {
            var letterI = guess[i];

            if (letterI == _word[i])
            {
                clues[i] = (letterI, Clue.CorrectPosition);
                markedLetterFreqs.IncrementValue(letterI);
            }
        }

        // Second pass - check for InWord/NotInWord letters
        for (var i = 0; i < _word.Length; ++i)
        {
            // Skip if we already marked the letter as CorrectPosition in the first pass
            if (clues[i] != default) continue;

            var letterI = guess[i];
            var letterFreq = markedLetterFreqs.IncrementValue(letterI);

            // The letter is in the word iff the word contains the letter (duh) AND we haven't seen the letter more times than it is in the word
            // This is so that when a guess contains repeated letters we can hint how many times that letter is in the word
            var clue = _word.Contains(letterI) && letterFreq <= _letterFreqs[letterI] ? Clue.InWord : Clue.NotInWord;
            clues[i] = (letterI, clue);
        }

        return clues;
    }

    private void UpdateLetterClues(WordWithClues wordWithClues)
    {
        foreach (var (c, clue) in wordWithClues)
        {
            if (clue == Clue.CorrectPosition)
            {
                // To overwrite InWord clues (and NotInWord clues when the guess has repeated letters)
                _letterClues[c] = clue;
            }
            else
            {
                // InWord and NotInWord clues should never overwrite existing clues because:
                // - NotInWord: in a guess with duplicate letters one might already be marked InWord or CorrectPosition
                // - InWord: the letter clue may already be CorrectPosition, and if there are duplicate letters in the guess the InWord clues come first
                _letterClues.TryAdd(c, clue);
            }
        }
    }

    private void GiveClues(WordWithClues wordWithClues)
    {
        for (int i=0; i < PROMPT.Length; ++i) Console.Write(' ');

        foreach (var (_, clue) in wordWithClues)
        {
            Console.BackgroundColor = clue.GetColour();
            Console.Write(clue.HintChar());
        }

        Console.ResetColor();
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
        // Validations ordered from quickest to slowest
        if (guess.Length != _word.Length) return false;
        if (!IsWord(guess)) return false;
        if (_wordlist.BinarySearch(guess) < 0) return false;

        return true;
    }

    private static int PaddedLength(char[] row) => row.Length * 2 - 1;

    private static bool IsWord(string str) => str.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));
}
