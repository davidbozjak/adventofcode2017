using System.Drawing;

var enhancementRules = new InputProvider<EnhancementRule?>("Input.txt", GetEnhancementRule).Where(w => w != null).Cast<EnhancementRule>().ToList();
var printer = new WorldPrinter();

var initialString = ".#./..#/###";
var initial = GetPixelsFromString(initialString, 0, 0);

var world = new SimpleWorld<Pixel>(initial);

for (int iteration = 1; iteration < 3; iteration++)
{
    if (world.Width != world.Height) throw new Exception("Always expecting world to be square");

    int size = world.Width;

    List<Pixel> pixels = new();
    int cellSize = size % 3 == 0 ? 3 : 2;
    int numOfCells = size / cellSize;

    for (int cell = 0; cell < numOfCells; cell++)
    {
        int originalTopLeft = cell * cellSize;

        var originalPixels = new List<Pixel>();

        for (int y = originalTopLeft; y < originalTopLeft + cellSize; y++)
        {
            for (int x = originalTopLeft; x < originalTopLeft + cellSize; x++)
            {
                originalPixels.Add(world.GetObjectAt(x, y));
            }
        }

        var ruleToApply = GetMatchingEnhancementRule(originalPixels);
        pixels.AddRange(GetPixelsFromString(ruleToApply.After, originalTopLeft, originalTopLeft));
    }

    world = new SimpleWorld<Pixel>(pixels);
    printer.Print(world);
    Console.WriteLine($"Iteration {iteration} Pixels On: {world.WorldObjects.Cast<Pixel>().Count(w => w.IsOn)}");
    Console.WriteLine("Press a key to iterate");
    Console.ReadKey();
}

static IEnumerable<Pixel> GetPixelsFromString(string input, int topLeftX, int topLeftY)
{
    var list = new List<Pixel>();

    int x = topLeftX;
    int y = topLeftY;

    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] == '/')
        {
            x = topLeftX;
            y++;
            continue;
        }

        var pixel = new Pixel
        {
            X = x++,
            Y = y,
            IsOn = input[i] == '#'
        };
        list.Add(pixel);
    }

    return list;
}

static bool GetEnhancementRule(string? input, out EnhancementRule? value)
{
    value = null;

    if (input == null) return false;

    var parts = input.Split(new[] { " ", "=>" }, StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length != 2) throw new Exception();

    value = new EnhancementRule(parts[0], parts[1]);

    return true;
}

static string GetStringFromPixels(IEnumerable<Pixel> pixels)
{
    var size = (int)Math.Sqrt(pixels.Count());

    var full = string.Join("", Enumerable.Range(0, size)
        .Select(i => pixels.Skip(i * size).Take(size).Select(w => w.CharRepresentation))
        .Select(w => string.Join("", w) + "/"));

    return full[..^1];
}

EnhancementRule GetMatchingEnhancementRule(IEnumerable<Pixel> pixels)
{
    // find a way to do transformations

    var transformedPixels = pixels;
    var str = GetStringFromPixels(transformedPixels);
    var rule = enhancementRules.FirstOrDefault(w => w.Before == GetStringFromPixels(transformedPixels));

    if (rule != null) return rule;
    throw new Exception();
}

record EnhancementRule(string Before, string After);

class Pixel : IWorldObject
{
    public Point Position => new(this.X, this.Y);

    public char CharRepresentation => this.IsOn ? '#' : '.';

    public int X { get; set; }

    public int Y { get; set; }

    public int Z => 1;

    public bool IsOn { get; set; }
}