string input = "jzgqcdpd";

int countUsed = 0;

List<List<int>> grid = new();
List<(int x, int y)> unclaimed = new();

for (int row = 0; row < 128; row++)
{
    var rowString = input + $"-{row}";
    var hash = KnotHash.GetKnotHash(rowString);

    string binarystring = string.Join(string.Empty,
      hash.Select(
        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
      )
    );

    var rowList = binarystring.Select(c => c == '1' ? 1 : 0).ToList();
    grid.Add(rowList);
    countUsed += rowList.Sum();

    for(int x = 0; x < rowList.Count; x++)
    {
        if (rowList[x] == 1)
            unclaimed.Add((x, row));
    }
}

Console.WriteLine($"Part 1: {countUsed}");

int regionCount = 0;
for (int paintId = 2; unclaimed.Count > 0; paintId++, regionCount++)
{
    Paint(paintId, grid, unclaimed.First(), unclaimed, new List<(int x, int y)>());
}

Console.WriteLine($"Part 2: {regionCount}");

static void Paint(int id, List<List<int>> grid, (int x, int y) cell, IList<(int x, int y)> unclaimed, IList<(int x, int y)> path)
{
    if (path.Contains(cell)) return;

    if (cell.x < 0 || cell.x >= 128) return;
    if (cell.y < 0 || cell.y >= 128) return;
    path.Add(cell);

    if (grid[cell.y][cell.x] == 1)
    {
        grid[cell.y][cell.x] = id;
        unclaimed.Remove(cell);

        Paint(id, grid, (cell.x - 1, cell.y), unclaimed, path);
        Paint(id, grid, (cell.x + 1, cell.y), unclaimed, path);
        Paint(id, grid, (cell.x, cell.y - 1), unclaimed, path);
        Paint(id, grid, (cell.x, cell.y + 1), unclaimed, path);
    }
}