var singleDigitIntParser = new SingleLineStringInputParser<int>(int.TryParse, str => str.Split('\t'));
var dataBank = new InputProvider<int>("Input.txt", singleDigitIntParser.GetValue).ToList();

var stateHash = GetDataBankStateHash(dataBank);
Dictionary<string, int> allSeenDataBankStates = new()
{
    { stateHash, 0 }
};

int step = 1;
for (; ; step++)
{
    int max = dataBank.Max();
    int indexOfMax = dataBank.IndexOf(max);
    dataBank[indexOfMax] = 0;

    for (int index = GetWrappedIndex(indexOfMax + 1); max > 0; index = GetWrappedIndex(index + 1), max -= 1)
    {
        dataBank[index]++;
    }

    stateHash = GetDataBankStateHash(dataBank);
    if (allSeenDataBankStates.ContainsKey(stateHash))
        break;

    allSeenDataBankStates.Add(stateHash, step);
}

Console.WriteLine($"Part 1: First duplicate hash after {step} steps");
Console.WriteLine($"Part 2: Loop length is: {step - allSeenDataBankStates[stateHash]}");

static string GetDataBankStateHash(IList<int> dataBankState) =>
    string.Join(',', dataBankState);

int GetWrappedIndex(int index) =>
    index >= dataBank.Count ? index - dataBank.Count : index;