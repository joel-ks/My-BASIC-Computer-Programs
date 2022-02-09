using System.Collections;
using System.Collections.Generic;

using CharWithClue = System.ValueTuple<char, WordleCmdLine.Clue>;

namespace WordleCmdLine;

class WordWithClues : IEnumerable
{
    private CharWithClue[] _chars;

    public CharWithClue this[int i]
    {
        get { return _chars[i]; }
        set { _chars[i] = value; }
    }

    public WordWithClues(int length)
    {
        _chars = new CharWithClue[length];
    }

    public IEnumerator<CharWithClue> GetEnumerator() 
    {
        foreach (CharWithClue c in _chars) yield return c;
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
