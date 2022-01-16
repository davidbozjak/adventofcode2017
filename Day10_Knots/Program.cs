var csvParser = new SingleLineStringInputParser<int>(int.TryParse, str => str.Split(',', StringSplitOptions.RemoveEmptyEntries));
var lengthInputs = new InputProvider<int>("Input.txt", csvParser.GetValue).ToList();

var part1List = Enumerable.Range(0, 256).ToList();
KnotList(part1List, lengthInputs, 0, 0);
Console.WriteLine($"Part 1: {part1List[0]}x{part1List[1]} = {part1List[0] * part1List[1]}");

var part2Lengths = new StreamReader("Input.txt").ReadLine().ToCharArray().Select(w => (int)w).ToList();
part2Lengths.AddRange(new[] { 17, 31, 73, 47, 23 });

int currentPosition = 0;
int skipSize = 0;
var part2List = Enumerable.Range(0, 256).ToList();

for (int i = 0; i < 64; i++)
{
    (currentPosition, skipSize) = KnotList(part2List, part2Lengths, currentPosition, skipSize);
}

var hash = CreateDenseHash(part2List);

Console.WriteLine($"Part 2: {hash}");

static (int currentPosition, int skipSize) KnotList<T>(IList<T> list, IEnumerable<int> lengths, int currentPosition, int skipSize)
{
    foreach (var length in lengths)
    {
        for (int i = currentPosition, j = Wrap(i + length - 1), steps = 0; steps < length / 2; steps++, i = Wrap(i + 1), j = Wrap(j - 1))
        {
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        currentPosition = Wrap(currentPosition + length + skipSize);
        skipSize++;
    }

    return (currentPosition, skipSize);

    int Wrap(int input)
    {
        int max = list.Count;
        while (input >= max) input -= max;
        while (input < 0) input += max;
        return input;
    }
}

static string CreateDenseHash(IList<int> list)
{
    var denseHash = new List<string>();

    for (int i = 0; i < 256; i += 16)
    {
        var hash = list[i];
        for (int j = 0; j < 15; j++)
            hash ^= list[i + j + 1];
        denseHash.Add(Convert.ToString(hash, 16).PadLeft(2, '0'));
    }

    return string.Join("", denseHash);
}