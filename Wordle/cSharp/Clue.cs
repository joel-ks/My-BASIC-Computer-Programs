using System;

namespace WordleCmdLine;

enum Clue
{
    NotInWord,
    InWord,
    CorrectPosition
}

static class ClueExtensions
{
    public static char HintChar(this Clue clue) {
        switch (clue)
        {
            case Clue.NotInWord: return ' ';
            case Clue.InWord: return '*';
            case Clue.CorrectPosition: return '^';
            default: throw new ArgumentException();
        }
    }

    public static ConsoleColor GetColour(this Clue clue)
    {
        switch (clue)
        {
            case Clue.CorrectPosition: return ConsoleColor.DarkGreen;
            case Clue.InWord: return ConsoleColor.DarkYellow;
            case Clue.NotInWord: return ConsoleColor.DarkGray;
            default: throw new ArgumentException();
        }
    }
}
