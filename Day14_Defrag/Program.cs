string input = "jzgqcdpd";

int countUsed = 0;

for (int row = 0; row < 128; row++)
{
    var rowString = input + $"-{row}";
    var hash = KnotHash.GetKnotHash(rowString);

    string binarystring = string.Join(string.Empty,
      hash.Select(
        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
      )
    );

    countUsed += binarystring.Select(c => c == '1' ? 1 : 0).Sum();
}

Console.WriteLine($"Part 1: {countUsed}");
