using System.Drawing;

class Pixel : IWorldObject
{
    public Point Position => new(this.X, this.Y);

    public char CharRepresentation => this.IsOn ? '#' : '.';

    public int X { get; init; }

    public int Y { get; init; }

    public int Z => 1;

    public bool IsOn { get; init; }
}