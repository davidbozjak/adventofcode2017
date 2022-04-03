using System.Drawing;

enum InfectionState { Clean, Weakened, Infected, Flagged };

class Cell : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation => this.State switch
    {
        InfectionState.Infected => '#',
        InfectionState.Weakened => 'W',
        InfectionState.Flagged => 'F',
        InfectionState.Clean => '.',
        _ => throw new Exception()
    };

    public int Z => 1;

    public Cell (int x, int y)
    {
        this.Position = new Point(x, y);
    }

    public InfectionState State { get; set; }
}