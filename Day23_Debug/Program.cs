var instructionStrings = new InputProvider<string?>("Input.txt", GetString).Cast<string>().ToList();

var program = new SingingProgram(instructionStrings);
program.Run();

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

