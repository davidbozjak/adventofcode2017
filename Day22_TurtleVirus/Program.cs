var tiles = GetInitialStateOfMap();
var initialWorld = new SimpleWorld<Cell>(tiles.AllCreatedInstances);
var virus = new VirusTurtle(tiles.GetOrCreateInstance((initialWorld.Width / 2, initialWorld.Height / 2)), tiles, false);

var printer = new WorldPrinter();

int numberOfInfections = Enumerable.Range(0, 10000)
    .Count(w => virus.Activate());

Console.WriteLine($"Part 1: {numberOfInfections}");

tiles = GetInitialStateOfMap();
virus = new VirusTurtle(tiles.GetOrCreateInstance((initialWorld.Width / 2, initialWorld.Height / 2)), tiles, true);


numberOfInfections = Enumerable.Range(0, 10000000)
    .Count(w => virus.Activate());

Console.WriteLine($"Part 2: {numberOfInfections}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

static UniqueFactory<(int x, int y), Cell> GetInitialStateOfMap()
{
    var tiles = new UniqueFactory<(int x, int y), Cell>(w => new Cell(w.x, w.y));
    var inputLines = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();

    for (int y = 0; y < inputLines.Count; y++)
    {
        for (int x = 0; x < inputLines[y].Length; x++)
        {
            var cell = tiles.GetOrCreateInstance((x, y));
            cell.State = inputLines[y][x] == '#' ? InfectionState.Infected : InfectionState.Clean;
        }
    }

    return tiles;
}