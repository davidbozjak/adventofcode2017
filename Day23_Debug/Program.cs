var instructionStrings = new InputProvider<string?>("Input.txt", GetString).Cast<string>().ToList();

var program = new SingingProgram(instructionStrings);
var countMultiple = program.Run("mul");

Console.WriteLine($"Part 1: {countMultiple}");

//betting that the algorithm is actually

int rez = 0;
for (long i = 106700; i <= 123700; i+= 17)
{
    if (!IsPrime(i))
        rez++;
}

Console.WriteLine($"Part 2: {rez}");

static bool IsPrime(long value)
{
    for (int devisor = 2; devisor < value; devisor++)
    {
        if (value % devisor == 0) return false;
    }

    return true;
}

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

