using System.Diagnostics;

var commaSeperatedStringLineParser = new SingleLineStringInputParser<string?>(GetString, str => str.Split(",", StringSplitOptions.RemoveEmptyEntries));
var directions = new InputProvider<string?>("Input.txt", commaSeperatedStringLineParser.GetValue).Where(w => w != null).Cast<string>().ToList();

var start = Hex.Get(0, 0);
Hex end = start;

int maxDistance = int.MinValue;

foreach (var direction in directions)
{
    end = direction switch
    {
        "n" => end.GetNorthNeighbour(),
        "s" => end.GetSouthNeighbour(),
        "nw" => end.GetNorthWestNeighbour(),
        "sw" => end.GetSouthWestNeighbour(),
        "ne" => end.GetNorthEastNeighbour(),
        "se" => end.GetSouthEastNeighbour(),
        _ => throw new Exception()
    };

    if (end.ShortestDistanceToStart > maxDistance)
    {
        maxDistance = end.ShortestDistanceToStart;
    }
}

Console.WriteLine($"Part 1: {end.ShortestDistanceToStart}");
Console.WriteLine($"Part 2: {maxDistance}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

[DebuggerDisplay("Row: {Row} Column: {Column}")]
class Hex : INode, IEquatable<Hex>
{
    //using "double-height" horizontal layout layout from https://www.redblobgames.com/grids/hexagons/

    static readonly UniqueFactory<(int row, int column), Hex> hexFactory = new(w => new Hex(w.row, w.column));

    public int Row { get; }
    public int Column { get; }

    public int Cost => 1;

    private readonly Cached<int> cachedShortestDistanceToStart;
    public int ShortestDistanceToStart => this.cachedShortestDistanceToStart.Value;

    private Hex(int row, int column)
    {
        if ((row + column) % 2 != 0) throw new Exception();

        this.Row = row;
        this.Column = column;

        this.cachedShortestDistanceToStart = new Cached<int>(GetShortestDistanceFromCenter);
    }

    public static Hex Get(int row, int column)
        => hexFactory.GetOrCreateInstance((row, column));

    public IEnumerable<Hex> GetNeighbours()
    {
        yield return GetNorthNeighbour();
        yield return GetSouthNeighbour();
        yield return GetNorthWestNeighbour();
        yield return GetSouthWestNeighbour();
        yield return GetNorthEastNeighbour();
        yield return GetSouthEastNeighbour();
    }

    public Hex GetNorthNeighbour()
        => Get(this.Row - 2, this.Column);

    public Hex GetSouthNeighbour()
        => Get(this.Row + 2, this.Column);

    public Hex GetNorthWestNeighbour()
        => Get(this.Row - 1, this.Column - 1);

    public Hex GetSouthWestNeighbour()
        => Get(this.Row + 1, this.Column - 1);

    public Hex GetNorthEastNeighbour()
        => Get(this.Row - 1, this.Column + 1);

    public Hex GetSouthEastNeighbour()
        => Get(this.Row + 1, this.Column + 1);

    private int GetShortestDistanceFromCenter()
    {
        //var path = AStarPathfinder.FindPath(Get(0, 0), this, _ => 0, hex => hex.GetNeighbours());
        //if (path == null) throw new Exception();
        //return path.Count - 1;

        return Distance(this, Get(0, 0));
    }

    public bool Equals(Hex? other)
    {
        if (other == null) return false;

        return this == other;
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static int Distance(Hex a, Hex b)
    {
        var colDistance = Math.Abs(a.Column - b.Column);
        var rowDistance = Math.Abs(a.Row - b.Row);

        return colDistance + Math.Max(0, (rowDistance - colDistance) / 2);
    }
}