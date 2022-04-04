static class TransformedWorldBuilder
{
    public static SimpleWorld<Pixel> CreateFlippedHorizontally(SimpleWorld<Pixel> world)
    {
        var flippedPixels = world.WorldObjects.Cast<Pixel>().Select(w => new Pixel
        {
            IsOn = w.IsOn,
            Y = w.Y,
            X = world.MaxX - w.X,
        });

        return new SimpleWorld<Pixel>(flippedPixels);
    }

    public static SimpleWorld<Pixel> CreateFlippedVertically(SimpleWorld<Pixel> world)
    {
        var flippedPixels = world.WorldObjects.Cast<Pixel>().Select(w => new Pixel
        {
            IsOn = w.IsOn,
            Y = world.MaxY - w.Y,
            X = w.X,
        });

        return new SimpleWorld<Pixel>(flippedPixels);
    }

    public static SimpleWorld<Pixel> CreateFlippedVerticallyAndHorizontally(SimpleWorld<Pixel> world)
    {
        var flippedPixels = world.WorldObjects.Cast<Pixel>().Select(w => new Pixel
        {
            IsOn = w.IsOn,
            Y = world.MaxY - w.Y,
            X = world.MaxX - w.X,
        });

        return new SimpleWorld<Pixel>(flippedPixels);
    }

    public static SimpleWorld<Pixel> CreateRotated90(SimpleWorld<Pixel> world)
    {        
        var rotatedPixels = new List<Pixel>();

        for (int row = world.MinY, column = world.MaxY; row <= world.MaxY; row++, column--)
        {
            for (int x = world.MinX; x <= world.MaxX; x++)
            {
                rotatedPixels.Add(new Pixel
                {
                    IsOn = world.GetObjectAt(x, row).IsOn,
                    X = column,
                    Y = x
                });
            }
        }

        return new SimpleWorld<Pixel>(rotatedPixels);
    }
}