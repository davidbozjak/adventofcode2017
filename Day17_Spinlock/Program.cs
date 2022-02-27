using System.Diagnostics;

var list = new CircularList<int>(0, 394);

while (list.Count < 2018)
{
    list.Insert(list.Count);
}

Console.WriteLine($"Part 1: Current: {list.CurrentValue} Next: {list.NextValue}");

while (list.Count < 50000000)
{
    list.Insert(list.Count);
}

list.MoveForwardToFindElement(0);

Console.WriteLine($"Part 2: Current: {list.CurrentValue} Next: {list.NextValue}");

[DebuggerDisplay("Current Value: {CurrentValue} Next Value: {NextValue} Total Elements: {Count}")]
class CircularList<T>
    where T : IComparable<T>
{
    protected CircularListElement<T> CurrentElement { get; private set; }

    public T CurrentValue => this.CurrentElement.Value;
    public T NextValue => this.CurrentElement.Next.Value;

    public int StepsAfterInsert { get; }

    public int Count { get; private set; }

    public CircularList(T initialElement, int steps)
    {
        this.CurrentElement = new CircularListElement<T>(initialElement);
        this.StepsAfterInsert = steps;
        this.Count = 1;
    }

    public void Insert(T value)
    {
        StepForward();

        var element = new CircularListElement<T>(value);
        element.Next = this.CurrentElement.Next;
        this.CurrentElement.Next = element;
        this.CurrentElement = element;
        this.Count++;
    }

    public void MoveForwardToFindElement(T valueToFind)
    {
        var initialCurrentElement = this.CurrentElement;

        while (!this.CurrentValue.Equals(valueToFind))
        {
            this.CurrentElement = this.CurrentElement.Next;

            if (this.CurrentElement == initialCurrentElement)
                throw new Exception("Value not found");
        }
    }

    private void StepForward()
    {
        for (int i = 0; i < this.StepsAfterInsert; i++)
        {
            this.CurrentElement = this.CurrentElement.Next;
        }
    }

    [DebuggerDisplay("{Value}")]
    protected class CircularListElement<U>
    {
        public CircularListElement<U> Next { get; set; }

        public U Value { get; }

        public CircularListElement(U element)
        {
            this.Value = element;
            this.Next = this;
        }
    }
}

