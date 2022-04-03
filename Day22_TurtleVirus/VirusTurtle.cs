using System.Drawing;

class VirusTurtle : IWorldObject
{
    public Point Position => this.CurrentCell.Position;

    public char CharRepresentation => 'X';

    public Cell CurrentCell { get; private set; }

    public int Z => 2;

    private readonly UniqueFactory<(int x, int y), Cell> tiles;

    enum Heading { Up = 1, Right = 2, Down = 3, Left = 4 };
    private Heading heading = Heading.Up;

    public bool IsMutated { get; }

    public VirusTurtle(Cell startingCell, UniqueFactory<(int x, int y), Cell> tiles, bool isMutated)
    {
        this.CurrentCell = startingCell;
        this.tiles = tiles;
        this.IsMutated = isMutated;
    }

    public bool Activate()
    {
        if (this.IsMutated)
        {
            this.heading = this.CurrentCell.State switch
            {
                InfectionState.Clean => this.heading - 1,
                InfectionState.Weakened => this.heading,
                InfectionState.Infected => this.heading + 1,
                InfectionState.Flagged => this.heading + 2,
                _ => throw new Exception()
            };

            this.CurrentCell.State = this.CurrentCell.State switch
            {
                InfectionState.Clean => InfectionState.Weakened,
                InfectionState.Weakened => InfectionState.Infected,
                InfectionState.Infected => InfectionState.Flagged,
                InfectionState.Flagged => InfectionState.Clean,
                _ => throw new Exception()
            };
        }
        else
        {
            this.heading += (this.CurrentCell.State == InfectionState.Infected ? 1 : -1);
            this.CurrentCell.State = this.CurrentCell.State == InfectionState.Infected ? InfectionState.Clean : InfectionState.Infected;
        }

        while ((int)this.heading > 4) this.heading -= 4;
        while ((int)this.heading < 1) this.heading += 4;

        bool hasCausedInfection = this.CurrentCell.State == InfectionState.Infected;

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
