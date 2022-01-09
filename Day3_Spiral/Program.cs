using System.Drawing;

// puzzle input
int requestedSquare = 289326;

int sideLength = 1;
bool extendSide = false;

int pozX = 0;
int pozY = 0;

State state = State.Right;

var createdCells = new List<Cell>() { new Cell(0, 0, 1) };

bool foundPart1 = false;
bool foundPart2 = false;

for (int steps = 1; !foundPart1 || !foundPart2;)
{
    int remainingSteps = requestedSquare - steps;
    int stepsMade = Math.Min(remainingSteps, sideLength);
    steps += stepsMade;

    for (int i = 0; i < stepsMade; i++)
    {
        pozX = state switch
        {
            State.Right => pozX + 1,
            State.Left => pozX - 1,
            _ => pozX
        };

        pozY = state switch
        {
            State.Up => pozY - 1,
            State.Down => pozY + 1,
            _ => pozY
        };

        if (!foundPart2)
        {
            var sum = createdCells.Where(w => Math.Abs(pozX - w.X) <= 1 && Math.Abs(pozY - w.Y) <= 1).Sum(w => w.Value);

            if (sum > requestedSquare)
            {
                Console.WriteLine($"Part 2: {sum}");
                foundPart2 = true;
            }

            createdCells.Add(new Cell(pozX, pozY, sum));
        }
    }

    if (steps == requestedSquare)
    {
        Console.WriteLine($"Part 1: {Math.Abs(pozX) + Math.Abs(pozY)}");
        foundPart1 = true;
    }

    state = state switch
    {
        State.Right => State.Up,
        State.Up => State.Left,
        State.Left => State.Down,
        State.Down => State.Right,
        _ => throw new Exception()
    };

    if (extendSide)
    {
        sideLength++;
        extendSide = false;
    }
    else
    {
        extendSide = true;
    }
}

enum State { Right, Up, Left, Down }

class Cell : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation => Value.ToString()[^1];

    public int X => this.Position.X;

    public int Y => this.Position.Y;

    public int Z => 1;

    public long Value { get; }

    public Cell (int x, int y, long value)
    {
        this.Position = new Point(x, y);
        this.Value = value;
    }
}