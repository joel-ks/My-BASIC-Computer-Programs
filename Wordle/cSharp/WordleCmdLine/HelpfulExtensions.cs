using System.Collections.Generic;
using System.Linq;

namespace WordleCmdLine;

static class HelpfulExtensions
{
    public static int IncrementValue<TKey>(this IDictionary<TKey, int> dict, TKey key)
    {
        if (!dict.TryGetValue(key, out int count)) count = 0;
        dict[key] = ++count;

        return count;
    }

    public static bool IsWord(this string str) => str.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));
}
