using System;

namespace WordleCmdLine;

static class ConsoleUtils
{
    public static void WriteColoured(char c, ConsoleColor bgColour)
    {
        var prevBgColour = Console.BackgroundColor;
        Console.BackgroundColor = bgColour;
        Console.Write(c);
        Console.BackgroundColor = prevBgColour;
    }

    public static void WriteColoured(string s, ConsoleColor bgColour)
    {
        var prevBgColour = Console.BackgroundColor;
        Console.BackgroundColor = bgColour;
        Console.Write(s);
        Console.BackgroundColor = prevBgColour;
    }

    public static void WriteLineColoured(string s, ConsoleColor bgColour)
    {
        var prevBgColour = Console.BackgroundColor;
        Console.BackgroundColor = bgColour;
        Console.WriteLine(s);
        Console.BackgroundColor = prevBgColour;
    }

    // Based on https://docs.microsoft.com/en-us/dotnet/api/system.consolekeyinfo.keychar?view=net-6.0#examples
    public static string ReadLineAllCaps()
    {
        var inputString = string.Empty;
        var initialCursorLeft = Console.CursorLeft;
        var initialCursorTop = Console.CursorTop;

        do
        {
            var keyInfo = Console.ReadKey(true);

            // Ignore if Ctrl or Alt is pressed
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0)
                continue;
            if ((keyInfo.Modifiers & ConsoleModifiers.Alt) != 0)
                continue;

            if (keyInfo.Key == ConsoleKey.Enter) break; // Finish reading when Enter is pressed
            else if (keyInfo.Key == ConsoleKey.Tab) continue; // Ignore tab key.
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                // Clear the input
                Console.SetCursorPosition(initialCursorLeft, initialCursorTop);
                Console.Write(MakeRepeatedCharString(' ', inputString.Length));
                Console.SetCursorPosition(initialCursorLeft, initialCursorTop);
                inputString = "";
            }
            else if (keyInfo.Key == ConsoleKey.Backspace) {
                // Backspace needs to be handled manually (but only if we have input)
                if (inputString.Length >= 1) {
                    inputString = inputString.Substring(0, inputString.Length - 1);
                    Console.Write("\b \b"); // Backspace - clear char - backspace again
                }
            }
            else
            {
                var c = keyInfo.KeyChar;

                // Ignore if char value is \u0000 (NUL) (means the key is not representable as a char, e.g. Insert, Home, Function keys)
                if (c == '\u0000') continue;

                // Echo and store char as upper case
                c = char.ToUpper(c);
                Console.Write(c);
                inputString += c;
            }
        } while (true);

        Console.WriteLine();
        return inputString;
    }

    public static void ClearCurrentLine()
    {
        Console.CursorLeft = 0;
        Console.Write(MakeRepeatedCharString(' ', Console.WindowWidth)); 
        Console.CursorLeft = 0;
    }

    public static string MakeRepeatedCharString(char c, int repetitions) => new string(c, repetitions);
}
