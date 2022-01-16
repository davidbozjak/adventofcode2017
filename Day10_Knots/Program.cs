var csvParser = new SingleLineStringInputParser<int>(int.TryParse, str => str.Split(',', StringSplitOptions.RemoveEmptyEntries));
var lengthInputs = new InputProvider<int>("Input.txt", csvParser.GetValue).ToList();

var list = Enumerable.Range(0, 256).ToList();
int currentPosition = 0;
int skipSize = 0;

foreach (var length in lengthInputs)
{
    for (int i = currentPosition, j = Wrap(i + length - 1, list.Count), steps = 0; steps < length / 2; steps++, i = Wrap(i + 1, list.Count), j = Wrap(j - 1, list.Count))
    {
        var tmp = list[i];
        list[i] = list[j];
        list[j] = tmp;
    }

    Console.WriteLine($"{string.Join(',', list)}");

    currentPosition = Wrap(currentPosition + length + skipSize, list.Count);
    skipSize++;
}

int Wrap(int input, int max)
{
    while (input >= max) input -= max;
    while (input < 0) input += max;
    return input;
}

Console.WriteLine($"Part 1: {list[0]}x{list[1]} = {list[0] * list[1]}");