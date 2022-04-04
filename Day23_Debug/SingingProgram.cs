using System.Diagnostics;

class SingingProgram
{
    private readonly UniqueFactory<string, Register> Registers;
    private readonly List<string> InstructionStrings;
    private int instructionIndex;
    private int multCount = 0;

    public SingingProgram(IEnumerable<string> instructions)
    {
        this.InstructionStrings = instructions.ToList();
        this.Registers = new UniqueFactory<string, Register>(name => new Register(name));
        this.instructionIndex = 0;
    }

    public void SetRegisterValue(string register, long value)
    {
        var r = Registers.GetOrCreateInstance(register);
        r.Value = value;
    }

    public bool Run()
    {
        bool ranAtLeastOneCommand = false;
        for (; instructionIndex >= 0 && instructionIndex < InstructionStrings.Count; instructionIndex++)
        {
            var instructionString = InstructionStrings[instructionIndex];

            var parts = instructionString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var instruction = parts[0];

            if (instruction == "set")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value = value;
            }
            else if (instruction == "sub")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value -= value;
            }
            else if (instruction == "mul")
            {
                Console.WriteLine($"Part 1: {++multCount}");
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value *= value;
            }
            else if (instruction == "jnz")
            {
                var valueX = GetValueOrRegisterValue(parts[1]);

                if (valueX != 0)
                {
                    var valueY = GetValueOrRegisterValue(parts[2]);

                    instructionIndex += (int)(valueY - 1);
                }
            }
            else throw new Exception("Unknown instruction");

            ranAtLeastOneCommand = true;
        }

        return ranAtLeastOneCommand;

        long GetValueOrRegisterValue(string valueOrRegister)
        {
            if (!long.TryParse(valueOrRegister, out long value))
            {
                var rx = Registers.GetOrCreateInstance(valueOrRegister);
                value = rx.Value;
            }

            return value;
        }
    }

    [DebuggerDisplay("{Name}:{Value}")]
    protected class Register
    {
        public string Name { get; }
        public long Value { get; set; } = 0;

        public Register(string name)
        {
            this.Name = name;
        }
    }
}