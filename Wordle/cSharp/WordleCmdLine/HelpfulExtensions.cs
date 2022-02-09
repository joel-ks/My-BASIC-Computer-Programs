using System.Collections.Generic;

namespace WordleCmdLine;

static class HelpfulExtensions
{
    public static int IncrementValue<TKey>(this IDictionary<TKey, int> dict, TKey key)
    {
        if (!dict.TryGetValue(key, out int count)) count = 0;
        dict[key] = ++count;

        return count;
    }
}
