// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var input = "Archive/Moshe Pictures/0001-Proccess/2019-07-02-תל אביב מהאוויר/4F5A2635.CR2";

Console.WriteLine($"Input is bidirectional: {HasBidiControlCharacters(input)}");

Console.ReadKey();



static bool HasBidiControlCharacters(string input)
{
    if (string.IsNullOrEmpty(input))
        return false;

    foreach (var c in input)
    {
        if (IsBidiControlChar(c))
            return true;
    }
    return false;
}
static bool IsBidiControlChar(char c)
{
    // check general range
    if (c < '\u200E' || c > '\u202E')
        return false;

    // check specific characters
    return (
        c == '\u200E' || // LRM
        c == '\u200F' || // RLM
        c == '\u202A' || // LRE
        c == '\u202B' || // RLE
        c == '\u202C' || // PDF
        c == '\u202D' || // LRO
        c == '\u202E'    // RLO
    );
}
