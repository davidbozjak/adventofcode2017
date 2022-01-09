var originalMaze = new InputProvider<int>("Input.txt", int.TryParse).ToList();

var part1Steps = RunMazeUntilEscape(originalMaze.ToList(), offset => offset + 1);

Console.WriteLine($"Part 1: Escape maze after {part1Steps} steps");

var part2Steps = RunMazeUntilEscape(originalMaze.ToList(), offset => offset >= 3 ? offset - 1 : offset + 1);

Console.WriteLine($"Part 2: Escape maze after {part2Steps} steps");

static int RunMazeUntilEscape(IList<int> maze, Func<int, int> indexScrambleFunc)
{
    int step = 0;

    for (int currentIndex = 0; currentIndex >= 0 && currentIndex < maze.Count; step++)
    {
        var offsetAtIndex = maze[currentIndex];
        maze[currentIndex] = indexScrambleFunc(offsetAtIndex);
        currentIndex += offsetAtIndex;
    }

    return step;
}