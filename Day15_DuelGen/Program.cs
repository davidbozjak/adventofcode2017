// sample data
//var generatorASpecs = new GeneratorSpecs(Factor: 16807, StartValue: 65);
//var generatorBSpecs = new GeneratorSpecs(Factor: 48271, StartValue: 8921);

// input data
var generatorASpecs = new GeneratorSpecs(Factor: 16807, StartValue: 116);
var generatorBSpecs = new GeneratorSpecs(Factor: 48271, StartValue: 299);

Generator generatorA = new(generatorASpecs);
Generator generatorB = new(generatorBSpecs);

var part1JudgeCount = Judge(40000000, generatorA, generatorB);
Console.WriteLine($"Part 1: {part1JudgeCount}");

generatorA = new(generatorASpecs) { AcceptanceCriteriaMask = 4 };
generatorB = new(generatorBSpecs) { AcceptanceCriteriaMask = 8 };

var part2JudgeCount = Judge(5000000, generatorA, generatorB);
Console.WriteLine($"Part 2: {part2JudgeCount}");

static long Judge(long numberOfTries, Generator generatorA, Generator generatorB)
{
    long truncateMask = 0b1111_1111_1111_1111;
    long judgeCount = 0;

    for (long i = 0; i < numberOfTries; i++)
    {
        var valueA = generatorA.GenerateNext();
        var valueB = generatorB.GenerateNext();

        long lowerA = valueA & truncateMask;
        long lowerB = valueB & truncateMask;

        if (lowerA == lowerB) judgeCount++;
    }

    return judgeCount;
}

record GeneratorSpecs (long Factor, long StartValue);

class Generator
{
    public long Factor { get; }

    public long AcceptanceCriteriaMask { get; init; } = 1;

    public long Value { get; private set; }

    public Generator(GeneratorSpecs specs)
    {
        this.Factor = specs.Factor;
        this.Value = specs.StartValue;
    }

    public long GenerateNext()
    {
        long nextValue = this.Value;

        do
        {
            nextValue = nextValue * Factor;
            nextValue = nextValue % 2147483647;

        } while ((nextValue % this.AcceptanceCriteriaMask) != 0);

        this.Value = nextValue;

        return this.Value;
    }
}