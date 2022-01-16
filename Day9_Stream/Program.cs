var input = new InputProvider<string?>("Input.txt", GetString).Where(w => w != null).Cast<string>().First();

bool isGarbage = false;
bool ignoreNext = false;
Group? currentGroup = null;
List<Group> allGroups = new();
int garabageCharacters = 0;

for (int index = 0; index < input.Length; index++)
{
    if (ignoreNext)
    {
        ignoreNext = false;
        continue;
    }

    char c = input[index];

    if (c == '!')
    {
        ignoreNext = true;
        continue;
    }

    if (isGarbage)
    {
        if (c == '>')
        {
            isGarbage = false;
        }
        else
        {
            garabageCharacters++;
        }
        
        continue;
    }

    if (c == '<')
    {
        isGarbage = true;
    }
    else if (c == '{')
    {
        Group? parent = currentGroup;

        currentGroup = new Group() { StartIndex = index, Parent = parent };
        allGroups.Add(currentGroup);

        parent?.SubGroups.Add(currentGroup);
    }
    else if (c == '}')
    {
        if (currentGroup == null) throw new Exception();

        currentGroup.EndIndex = index;
        currentGroup = currentGroup.Parent;
    }
}

Console.WriteLine($"Part 1: {allGroups.Sum(w => w.Score)}");
Console.WriteLine($"Part 2: {garabageCharacters} garbage characters in total");

static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

class Group
{
    public int StartIndex { get; init; }
    public int EndIndex { get; set; }

    public int Score => (Parent?.Score ?? 0) + 1;

    public List<Group> SubGroups { get; } = new List<Group>();
    public Group? Parent { get; init; }
}