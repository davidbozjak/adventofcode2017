using System.Text.RegularExpressions;

var components = new InputProvider<Component?>("Input.txt", GetComponent).Where(w => w != null).Cast<Component>().ToList();

Console.WriteLine($"Part 1: { GetStrengthForBridge(GetStrongestBridge(new List<Component>(), 0, components, GetStrengthForBridge))}");
Console.WriteLine($"Part 2: { GetStrengthForBridge(GetStrongestBridge(new List<Component>(), 0, components, GetStrengthForLongestBridge))}");

static IEnumerable<Component> GetStrongestBridge(List<Component> baseBridge, int lastPin, List<Component> components, Func<IEnumerable<Component>, int> evalFunc)
{
    List<(IEnumerable<Component> bridge, int strength)> possibleBridges = new()
    {
        (baseBridge, evalFunc(baseBridge))
    };

    foreach (var component in components)
    {
        if (baseBridge.Contains(component)) continue;

        if (component.PortA != lastPin && component.PortB != lastPin)
            continue;

        var bridge = baseBridge.ToList();
        bridge.Add(component);

        var nextPin = component.PortA == lastPin ? component.PortB : component.PortA;

        var maxFollowingBridge = GetStrongestBridge(bridge, nextPin, components, evalFunc);
        possibleBridges.Add((maxFollowingBridge, evalFunc(maxFollowingBridge)));
    }

    return possibleBridges.OrderByDescending(w => w.strength).Select(w => w.bridge).First();
}

static bool GetComponent(string? input, out Component? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"-?\d+");
    var numbers = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    value = new Component(numbers[0], numbers[1]);

    return true;
}

static int GetStrengthForBridge(IEnumerable<Component> bridge) =>
    bridge.Any() ? bridge.Sum(w => w.PortA + w.PortB) : 0;

static int GetStrengthForLongestBridge(IEnumerable<Component> bridge) =>
    bridge.Any() ? ((bridge.Count() * 1000) + bridge.Sum(w => w.PortA + w.PortB)) : 0;

record Component(int PortA, int PortB);