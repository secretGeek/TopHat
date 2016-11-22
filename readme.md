# TopHat

Snippet assistant for presentations on Windows.

![screenshot](screenshot.png)

Use next and previous to move through a set of snippets that are loaded from a file. (Snippets are assumed to be separated by '#' comments)

The snippet file can be set by passing a commandline parameter. (If not set there, you are asked to select a file)

You can also specify a set of ReplaceThis`WithThis token pairs. WHat does that mean? Allow me to explain...

Your script file might mention, for example, a particular IP Address over and over. But that IP Address is really
just a placeholder for the actual IP Address you will use on the day. If so, set up a "ReplaceThis`WithThis" token pair, that will be used to replace the place holder IP Address with the actual IP Address.


## Commandline help:

Top Hat
Snippet assistant for presentations on Windows.

Usage: TopHat.exe [options]

Options:
  -s, --snippets=VALUE       snippet file (separated by '#' comments)
  -r, --replacements=ReplaceThis
                             comma separated set of ReplaceThis`WithThis token
                               pairs;
  -?, -h, --help             show this message and exit
 