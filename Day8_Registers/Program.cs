using System.Text.RegularExpressions;

var registers = new UniqueFactory<string, Register>(name => new Register(name));
var instructions = new InputProvider<Instruction?>("Input.txt", GetInstruction).Where(w => w != null).Cast<Instruction>().ToList();

foreach (var instruction in instructions)
{
    var conditionParts = instruction.Condition.Split(" ", StringSplitOptions.RemoveEmptyEntries);

    if (conditionParts.Length != 3) throw new Exception();
    
    var registerName = conditionParts[0];
    Register conditionedRegister = registers.GetOrCreateInstance(registerName);
    var conditionValue = int.Parse(conditionParts[2]);

    bool eval = conditionParts[1] switch
    {
        ">" => conditionedRegister.Value > conditionValue,
        ">=" => conditionedRegister.Value >= conditionValue,
        "<" => conditionedRegister.Value < conditionValue,
        "<=" => conditionedRegister.Value <= conditionValue,
        "==" => conditionedRegister.Value == conditionValue,
        "!=" => conditionedRegister.Value != conditionValue,
        _ => throw new Exception()
    };

    if (eval)
    {
        instruction.Register.Value = instruction.Increase ?
            instruction.Register.Value + instruction.Amount :
            instruction.Register.Value - instruction.Amount;
    }
}

Console.WriteLine($"Part 1: Largest value in any register: {registers.AllCreatedInstances.Select(w => w.Value).Max()}");
Console.WriteLine($"Part 2: Largest value at any time: {registers.AllCreatedInstances.SelectMany(w => w.HistoricalValues).Max()}");

bool GetInstruction(string? input, out Instruction? value)
{
    value = null;

    if (input == null) return false;

    var registerName = input[..input.IndexOf(' ')];
    var register = registers.GetOrCreateInstance(registerName);
    var increase = input.Contains("inc");
    var amount = int.Parse(new Regex(@"-?\d+").Match(input).Value);

    value = new Instruction(register, increase, amount, input[(input.IndexOf("if") + 3)..]);

    return true;
}

record Instruction(Register Register, bool Increase, int Amount, string Condition);

class Register
{
    public string Name { get; }

    private int currentValue;
    private readonly List<int> allHistoricalValues = new();

    public int Value
    {
        get => this.currentValue;
        set
        {
            this.currentValue = value;
            this.allHistoricalValues.Add(this.currentValue);
        }
    }
    public IReadOnlyCollection<int> HistoricalValues => this.allHistoricalValues.AsReadOnly();

    public Register(string name)
    {
        this.Name = name;
        this.Value = 0;
    }
}