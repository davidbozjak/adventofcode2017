using System.Text.RegularExpressions;

var scanners = new InputProvider<Scanner?>("Input.txt", GetScanner).Where(w => w != null).Cast<Scanner>().ToList();
int maxDepth = scanners.Max(w => w.Depth);

var caughtOccasionsNaive = RunSimulation(scanners);

Console.WriteLine($"Part 1: {caughtOccasionsNaive.Select(w => w.Depth * w.Range).Sum()}");

for (long delay = 1; ; delay++)
{
    scanners.ForEach(w => w.MakeStep());
    var caughtOccasions = RunSimulation(scanners, true);

    if (!caughtOccasions.Any())
    {
        Console.WriteLine($"Part 2: {delay}");
        break;
    }
}

IList<Scanner> RunSimulation(IEnumerable<Scanner> scannerInitialState, bool earlyOut = false)
{
    var scanners = scannerInitialState.Select(w => w.Clone()).ToList();
    var caughtOccasions = new List<Scanner>();

    for (int packetDepth = 0; packetDepth <= maxDepth; packetDepth++)
    {
        var scannerAtLayer = scanners.Where(w => w.Depth == packetDepth).FirstOrDefault();

        if (scannerAtLayer != null)
        {
            if (scannerAtLayer.CurrentPosition == 0)
            {
                caughtOccasions.Add(scannerAtLayer);

                if (earlyOut) break;
            }
        }

        scanners.ForEach(w => w.MakeStep());
    }

    return caughtOccasions;
}

static bool GetScanner(string? input, out Scanner? value)
{
    value = null;

    if (input == null) return false;
    
    Regex numRegex = new(@"\d+");

    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    if (numbers.Length != 2) throw new Exception();

    value = new Scanner(numbers[0], numbers[1]);

    return true;
}

class Scanner
{
    public int Depth { get; }
    public int Range { get; }

    public int CurrentPosition { get; private set; }

    private bool directionAscending = true;

    public Scanner(int depth, int range)
    {
        this.Depth = depth;
        this.Range = range;

        this.CurrentPosition = 0;
        this.directionAscending = true;
    }

    public void MakeStep()
    {
        this.CurrentPosition += this.directionAscending ? 1 : -1;

        if (this.CurrentPosition == this.Range - 1)
            this.directionAscending = false;

        if (this.CurrentPosition == 0)
            this.directionAscending = true;
    }

    public Scanner Clone()
    {
        var scanner = new Scanner(this.Depth, this.Range)
        {
            directionAscending = this.directionAscending,
            CurrentPosition = this.CurrentPosition
        };
        return scanner;
    }
}