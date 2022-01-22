using System.Text.RegularExpressions;

var factory = new UniqueFactory<int, Agent>(id => new Agent(id));
var agents = new InputProvider<Agent?>("Input.txt", GetAgent).Where(w => w != null).Cast<Agent>().ToList();

var programToFind = factory.GetOrCreateInstance(0);
Console.WriteLine($"Part 1: {agents.Where(w => w.CanReach(programToFind)).Count()}");

var unsorted = agents.ToList();
var groups = new List<HashSet<Agent>>();

while (unsorted.Count > 0)
{
    var agent = unsorted.First();
    unsorted.Remove(agent);

    var reachable = agent.GetAllReachableAgents();
    unsorted.RemoveAll(reachable.Contains);

    groups.Add(reachable);
}

Console.WriteLine($"Part 2: {groups.Count}");

bool GetAgent(string? input, out Agent? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");

    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToList();

    value = factory.GetOrCreateInstance(numbers[0]);
    value.AddAgents(numbers.Skip(1).Select(w => factory.GetOrCreateInstance(w)));

    return true;
}