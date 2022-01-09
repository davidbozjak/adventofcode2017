using System.Diagnostics;

var factory = new UniqueFactory<string, ProgramElement>(name => new ProgramElement(name));

var programs = new InputProvider<ProgramElement?>("Input.txt", GetProgram).Where(w => w != null).Cast<ProgramElement>().ToList();

var bottom = programs.Where(w => w.Parent == null).First();

Console.WriteLine($"Part 1: {bottom.Name} (Count: {programs.Where(w => w.Parent == null).Count()})");

var unbalancedDiscs = programs.Where(w => !w.IsBalanced).ToList();
var problematicDiscParent = unbalancedDiscs.OrderByDescending(w => w.Level).First();
var problematicDisc = problematicDiscParent.Children
    .Where(w => w.TotalWeight != problematicDiscParent.Children.Select(w => w.TotalWeight).GetMostFrequentElement())
    .First();

var newWeight = problematicDisc.Parent.Children.Select(w => w.TotalWeight).GetMostFrequentElement() - problematicDisc.Children.Sum(w => w.TotalWeight);

Console.WriteLine($"Part 2: {newWeight}");

bool GetProgram(string? input, out ProgramElement? value)
{
    value = null;

    if (input == null) return false;

    var indexOfSeperator = input.IndexOf("->");

    value = factory.GetOrCreateInstance(input[0..(input.IndexOf('(') - 1)]);

    int weight = int.Parse(input[(input.IndexOf('(') + 1)..input.IndexOf(')')]);
    value.Weight = weight;

    if (indexOfSeperator > -1)
    {
        var subprograms = input[(indexOfSeperator + 2)..].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var name in subprograms)
        {
            var subprogram = factory.GetOrCreateInstance(name);
            subprogram.Parent = value;
            value.Children.Add(subprogram);
        }
    }

    return true;
}

[DebuggerDisplay("Name: {Name} Level: {Level} TotalWeight: {TotalWeight}")]
class ProgramElement
{
    public string Name { get; }

    public int Weight { get; set; }

    public int TotalWeight => Weight + this.Children.Sum(w => w.TotalWeight);

    public bool IsBalanced => this.Children.Count == 0 || this.Children.All(w => w.TotalWeight == this.Children[0].TotalWeight);

    public ProgramElement? Parent { get; set; }

    public int Level => this.Parent == null ? 0 : this.Parent.Level + 1;

    public List<ProgramElement> Children { get; } = new();

    public ProgramElement(string name)
    {
        this.Name = name;
    }
}