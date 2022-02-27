var instructionStrings = new InputProvider<string?>("Input.txt", GetString).Cast<string>().ToList();

long playedFrequency = 0;
Func<long, long?> listenOrTerminate = value =>
{
    if (value != 0)
    {
        return null;
    }
    return playedFrequency;
};

var program = new SingingProgram(instructionStrings, frq => playedFrequency = frq, listenOrTerminate);
program.Run();

Console.WriteLine($"Part 1: First NonZero frequency: {playedFrequency}");

var program0Queue = new Queue<long>();
var program1Queue = new Queue<long>();

int program0SendCount = 0;
int program1SendCount = 0;

var program0 = new SingingProgram(instructionStrings, frq => { program1Queue.Enqueue(frq); program0SendCount++; }, value => program0Queue.Count > 0 ? program0Queue.Dequeue() : null);
program0.SetRegisterValue("p", 0);

var program1 = new SingingProgram(instructionStrings, frq => { program0Queue.Enqueue(frq); program1SendCount++; }, value => program1Queue.Count > 0 ? program1Queue.Dequeue() : null);
program1.SetRegisterValue("p", 1);

bool ranAtleastOne;
do
{
    ranAtleastOne = false;
    ranAtleastOne |= program0.Run();
    ranAtleastOne |= program1.Run();
} while (ranAtleastOne);

Console.WriteLine($"Part 2: {program1SendCount}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

