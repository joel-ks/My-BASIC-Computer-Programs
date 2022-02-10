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
