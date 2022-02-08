Wordlist from https://gist.github.com/zneak/53f885f1fc5856741cb4

Use the following regexes to sanitise to only valid 5 letter words:
- `^(\W+|.{0,4}|.{6,})$` -> `\n`
    - Anything that's not alphanumeric (`\W+`)
    - Anything between 0 and 4 characters in length (`.{0,4}`)
    - Anything 6 or more characters in length (`.{6,}`)
- `\n+` -> `\n`
    - Replace all consecutive line breaks with a single line break
