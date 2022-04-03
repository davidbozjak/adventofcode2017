var enhancementRules = new InputProvider<EnhancementRule?>("Input.txt", GetEnhancementRule).Where(w => w != null).Cast<EnhancementRule>().ToList();
Dictionary<string, EnhancementRule> pixelTileMemoizationDict = new();

var printer = new WorldPrinter();

var initialString = ".#./..#/###";
var initial = GetPixelsFromString(initialString, 0, 0);

var world = new SimpleWorld<Pixel>(initial);

bool printOut = false;

for (int iteration = 1; iteration < 19; iteration++)
{
    if (world.Width != world.Height) throw new Exception("Always expecting world to be square");

    int size = world.Width;

    List<Pixel> pixels = new();
    int sourceCellSize = size % 2 == 0 ? 2 : 3;
    int destinationCellSize = size % 2 == 0 ? 3 : 4;
    int numOfCells = size / sourceCellSize;

    for (int cellX = 0; cellX < numOfCells; cellX++)
    {
        for (int cellY = 0; cellY < numOfCells; cellY++)
        {
            int sourceTopLeftX = cellX * sourceCellSize;
            int sourceTopLeftY = cellY * sourceCellSize;

            var originalPixels = new List<Pixel>();

            for (int y = sourceTopLeftY, stepY = 0; stepY < sourceCellSize; y++, stepY++)
            {
                for (int x = sourceTopLeftX, stepX = 0; stepX < sourceCellSize; x++, stepX++)
                {
                    originalPixels.Add(world.GetObjectAt(x, y));
                }
            }

            int destinationTopLeftX = cellX * destinationCellSize;
            int destinationTopLeftY = cellY * destinationCellSize;

            var ruleToApply = GetMatchingEnhancementRule(originalPixels);
            pixels.AddRange(GetPixelsFromString(ruleToApply.After, destinationTopLeftX, destinationTopLeftY));
        }
    }

    world = new SimpleWorld<Pixel>(pixels);
    
    Console.WriteLine($"Iteration {iteration} Pixels On: {world.WorldObjects.Cast<Pixel>().LongCount(w => w.IsOn)}");
    
    if (printOut)
    {
        printer.Print(world);
        Console.WriteLine("Press a key to iterate");
        Console.ReadKey();
    }
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
    var key = GetStringFromPixels(pixels);
    if (pixelTileMemoizationDict.ContainsKey(key))
        return pixelTileMemoizationDict[key];

    foreach (var transformedPixels in GetAllTransformations(pixels))
    {
        var rule = enhancementRules.FirstOrDefault(w => w.Before == GetStringFromPixels(transformedPixels));

        if (rule != null)
        {
            pixelTileMemoizationDict.Add(key, rule);
            return rule;
        }
    }

    //always expecing to find a match
    throw new Exception();
}

static IEnumerable<IEnumerable<Pixel>> GetAllTransformations(IEnumerable<Pixel> pixels)
{
    var originalWorld = new SimpleWorld<Pixel>(pixels);
    var noFlipWorld = new SimpleWorld<Pixel>(pixels);

    yield return noFlipWorld.WorldObjectsT;

    for (int i = 0; i < 3; i++)
    {
        noFlipWorld = TransformedWorldBuilder.CreateRotated90(noFlipWorld);
        yield return noFlipWorld.WorldObjectsT;
    }

    var horizontalFlipWorld = TransformedWorldBuilder.CreateFlippedHorizontally(originalWorld);
    yield return horizontalFlipWorld.WorldObjectsT;

    for (int i = 0; i < 3; i++)
    {
        horizontalFlipWorld = TransformedWorldBuilder.CreateRotated90(horizontalFlipWorld);
        yield return horizontalFlipWorld.WorldObjectsT;
    }

    var verticalFlipWorld = TransformedWorldBuilder.CreateFlippedVertically(originalWorld);
    yield return verticalFlipWorld.WorldObjectsT;

    for (int i = 0; i < 3; i++)
    {
        verticalFlipWorld = TransformedWorldBuilder.CreateRotated90(verticalFlipWorld);
        yield return verticalFlipWorld.WorldObjectsT;
    }

    var bothFlipWorld = TransformedWorldBuilder.CreateFlippedVerticallyAndHorizontally(originalWorld);
    yield return bothFlipWorld.WorldObjectsT;

    for (int i = 0; i < 3; i++)
    {
        bothFlipWorld = TransformedWorldBuilder.CreateRotated90(bothFlipWorld);
        yield return bothFlipWorld.WorldObjectsT;
    }
}

record EnhancementRule(string Before, string After);