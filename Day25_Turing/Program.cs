int currentIndex = 0;
State state = State.A;
Dictionary<int, int> tape = new();

for (int i = 0; i < 12656374; i++)
{
    if (state == State.A)
    {
        if (CurrentValue() == 0)
        {
            tape[currentIndex] = 1;
            currentIndex++;
            state = State.B;
        }
        else if (CurrentValue() == 1)
        {
            tape[currentIndex] = 0;
            currentIndex--;
            state = State.C;
        }
        else throw new Exception("Unexpected current value");
    }
    else if (state == State.B)
    {
        if (CurrentValue() == 0)
        {
            tape[currentIndex] = 1;
            currentIndex--;
            state = State.A;
        }
        else if (CurrentValue() == 1)
        {
            tape[currentIndex] = 1;
            currentIndex--;
            state = State.D;
        }
        else throw new Exception("Unexpected current value");
    }
    else if (state == State.C)
    {
        if (CurrentValue() == 0)
        {
            tape[currentIndex] = 1;
            currentIndex++;
            state = State.D;
        }
        else if (CurrentValue() == 1)
        {
            tape[currentIndex] = 0;
            currentIndex++;
            state = State.C;
        }
        else throw new Exception("Unexpected current value");
    }
    else if (state == State.D)
    {
        if (CurrentValue() == 0)
        {
            tape[currentIndex] = 0;
            currentIndex--;
            state = State.B;
        }
        else if (CurrentValue() == 1)
        {
            tape[currentIndex] = 0;
            currentIndex++;
            state = State.E;
        }
        else throw new Exception("Unexpected current value");
    }
    else if (state == State.E)
    {
        if (CurrentValue() == 0)
        {
            tape[currentIndex] = 1;
            currentIndex++;
            state = State.C;
        }
        else if (CurrentValue() == 1)
        {
            tape[currentIndex] = 1;
            currentIndex--;
            state = State.F;
        }
        else throw new Exception("Unexpected current value");
    }
    else if (state == State.F)
    {
        if (CurrentValue() == 0)
        {
            tape[currentIndex] = 1;
            currentIndex--;
            state = State.E;
        }
        else if (CurrentValue() == 1)
        {
            tape[currentIndex] = 1;
            currentIndex++;
            state = State.A;
        }
        else throw new Exception("Unexpected current value");
    }
    else throw new Exception("Unexpected state");
}

Console.WriteLine($"Part 1: {tape.Values.Sum()}");

int CurrentValue()
{
    if (!tape.ContainsKey(currentIndex))
    {
        tape[currentIndex] = 0;
    }

    return tape[currentIndex];
}

enum State { A, B, C, D, E, F }