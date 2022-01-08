using System.Text.RegularExpressions;

var rows = new InputProvider<int[]?>("Input.txt", GetSpreadsheetRow).Where(w => w != null).Cast<int[]>().ToList();

var rowChecksums = rows.Select(w => w.Max() - w.Min());

Console.WriteLine($"Part 1: {rowChecksums.Sum()}");

var rowDevisors = rows.Select(OnlyDevisorFromList);

Console.WriteLine($"Part 2: {rowDevisors.Sum()}");

static bool GetSpreadsheetRow(string? input, out int[]? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"\d+");

    value = numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray();

    return true;
}

static int OnlyDevisorFromList(int[] values)
{
    for (int i = 0; i < values.Length; i++)
    {
        for (int j = 0; j < values.Length; j++) 
        {
            if (i == j) continue;

            if (values[i] % values[j] == 0)
                return values[i] / values[j];
        }
    }

    throw new Exception("Assumption failed, no number devides another number from the list without a remainder");
}