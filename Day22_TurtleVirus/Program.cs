using System.Drawing;

var tiles = new UniqueFactory<(int x, int y), Cell>(w => new Cell(w.x, w.y));
var inputLines = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().ToList();

for(int y = 0; y < inputLines.Count; y++)
{
    for (int x = 0; x < inputLines[y].Length; x++)
    {
        var cell = tiles.GetOrCreateInstance((x, y));
        cell.IsInfected = inputLines[y][x] == '#';
    }
}

var virus = new VirusTurtle(tiles.GetOrCreateInstance((inputLines[0].Length / 2, inputLines.Count / 2)), tiles);

int numberOfInfections = 0;
for (int iteration = 1; iteration <= 10000; iteration++)
{
    if (virus.Activate())
    {
        numberOfInfections++;
    }
}

Console.WriteLine($"Part 1: Activations after 1000 iterations: {numberOfInfections}");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

enum Heading { Up = 1, Right = 2, Down = 3, Left = 4};

class VirusTurtle : IWorldObject
{
    public Point Position => this.CurrentCell.Position;

    public char CharRepresentation => 'X';

    public Cell CurrentCell { get; private set; }

    public int Z => 2;

    private readonly UniqueFactory<(int x, int y), Cell> tiles;

    private Heading heading = Heading.Up;


    public VirusTurtle(Cell startingCell, UniqueFactory<(int x, int y), Cell> tiles)
    {
        this.CurrentCell = startingCell;
        this.tiles = tiles;
    }

    public bool Activate()
    {
        this.heading += (this.CurrentCell.IsInfected ? 1 : -1);

        if ((int)this.heading == 0) this.heading = (Heading)4;
        if ((int)this.heading == 5) this.heading = (Heading)1;

        this.CurrentCell.IsInfected = !this.CurrentCell.IsInfected;

        bool hasCausedInfection = this.CurrentCell.IsInfected;

        var newLocation = this.heading switch
        {
            Heading.Up => (this.CurrentCell.Position.X, this.CurrentCell.Position.Y - 1),
            Heading.Down => (this.CurrentCell.Position.X, this.CurrentCell.Position.Y + 1),
            Heading.Left => (this.CurrentCell.Position.X - 1, this.CurrentCell.Position.Y),
            Heading.Right => (this.CurrentCell.Position.X + 1, this.CurrentCell.Position.Y),
            _ => throw new Exception()
        };

        this.CurrentCell = tiles.GetOrCreateInstance(newLocation);

        return hasCausedInfection;
    }
}

class Cell : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation => IsInfected ? '#' : '.';

    public int Z => 1;

    public Cell (int x, int y)
    {
        this.Position = new Point(x, y);
    }

    public bool IsInfected { get; set; }
}