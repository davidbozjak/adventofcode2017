using System.Diagnostics;

class SingingProgram
{
    private readonly UniqueFactory<string, Register> Registers;
    private readonly List<string> InstructionStrings;
    private readonly Action<long> SingAction;
    private readonly Func<long, long?> ListenFunc;
    private int instructionIndex;

    public SingingProgram(IEnumerable<string> instructions, Action<long> singAction, Func<long, long?> listenFunc)
    {
        this.InstructionStrings = instructions.ToList();
        this.SingAction = singAction;
        this.ListenFunc = listenFunc;
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

            if (instruction == "snd")
            {
                var value = GetValueOrRegisterValue(parts[1]);

                this.SingAction(value);
            }
            else if (instruction == "set")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value = value;
            }
            else if (instruction == "add")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value += value;
            }
            else if (instruction == "mul")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value *= value;
            }
            else if (instruction == "mod")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = GetValueOrRegisterValue(parts[2]);

                r.Value %= value;
            }
            else if (instruction == "rcv")
            {
                var r = Registers.GetOrCreateInstance(parts[1]);

                var value = this.ListenFunc(r.Value);

                if (value == null)
                { 
                    return ranAtLeastOneCommand;
                }

                r.Value = value.Value;
            }
            else if (instruction == "jgz")
            {
                var valueX = GetValueOrRegisterValue(parts[1]);

                if (valueX > 0)
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