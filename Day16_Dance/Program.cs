var commaSeparetedStringParser = new SingleLineStringInputParser<string?>(GetString, str => str.Split(",", StringSplitOptions.RemoveEmptyEntries));
var commands = new InputProvider<string?>("Input.txt", commaSeparetedStringParser.GetValue).Cast<string>().ToList();

int noOfDancers = 16;
var startingPosition = Enumerable.Range('a', noOfDancers).Select(w => (char)w).ToList();
var dancers = ExecuteDance(startingPosition, commands);

var resultOfFirstFullDance = GetStringRepresentation(dancers);
Console.WriteLine($"Part 1: {resultOfFirstFullDance}");

var configurations = new List<string>()
{
    GetStringRepresentation(startingPosition),
    resultOfFirstFullDance,
};

bool hasJumpedAhead = false;
long totalNumberOfDancesToDo = (long)1e9; // 1000000000

for (long noOfDances = 1; noOfDances < totalNumberOfDancesToDo; noOfDances++)
{
    dancers = ExecuteDance(dancers, commands);

    if (!hasJumpedAhead)
    {
        var afterDanceConfiguration = GetStringRepresentation(dancers);

        if (!configurations.Contains(afterDanceConfiguration))
        {
            configurations.Add(afterDanceConfiguration);
        }
        else
        {
            var indexOfLastOccurance = configurations.IndexOf(afterDanceConfiguration);
            long cycleLength = noOfDances + 1 - indexOfLastOccurance;

            noOfDances += cycleLength * ((totalNumberOfDancesToDo - noOfDances) / cycleLength);

            hasJumpedAhead = true;
        }
    }
}

Console.WriteLine($"Part 2: {GetStringRepresentation(dancers)}");

static List<char> ExecuteDance(List<char> initialPosition, List<string> commands)
{
    var dancers = initialPosition.ToList();

    foreach (var command in commands)
    {
        switch (command[0])
        {
            case 's':
                Spin(dancers, int.Parse(command[1..]));
                break;
            case 'x':
                var indexOfSlash = command.IndexOf('/');
                Exchange(dancers, int.Parse(command[1..indexOfSlash]), int.Parse(command[(indexOfSlash + 1)..]));
                break;
            case 'p':
                Partner(dancers, command[1], command[3]);
                break;
        }
    }

    return dancers;
}

static void Spin(List<char> dancers, int count)
{
    for (int i = 0; i < count; i++)
    {
        var last = dancers.Last();
        dancers.RemoveAt(dancers.Count - 1);
        dancers.Insert(0, last);
    }
}

static void Exchange(List<char> dancers, int firstIndex, int secondIndex)
{
    var first = dancers[firstIndex];
    var second = dancers[secondIndex];

    dancers[firstIndex] = second;
    dancers[secondIndex] = first;
}

static void Partner(List<char> dancers, char first, char second)
{
    var indexOfFirst = dancers.IndexOf(first);
    var indexOfSecond = dancers.IndexOf(second);

    dancers[indexOfFirst] = second;
    dancers[indexOfSecond] = first;
}

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

static string GetStringRepresentation(List<char> dancers) =>
    string.Join("", dancers);