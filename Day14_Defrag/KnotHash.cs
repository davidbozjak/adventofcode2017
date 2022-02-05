static class KnotHash
{
    public static string GetKnotHash(string inputString)
    {
        var lengths = inputString.ToCharArray().Select(w => (int)w).ToList();
        lengths.AddRange(new[] { 17, 31, 73, 47, 23 });

        int currentPosition = 0;
        int skipSize = 0;
        var part2List = Enumerable.Range(0, 256).ToList();

        for (int i = 0; i < 64; i++)
        {
            (currentPosition, skipSize) = KnotList(part2List, lengths, currentPosition, skipSize);
        }

        return CreateDenseHash(part2List);
    }

    static (int currentPosition, int skipSize) KnotList<T>(IList<T> list, IEnumerable<int> lengths, int currentPosition, int skipSize)
    {
        foreach (var length in lengths)
        {
            for (int i = currentPosition, j = Wrap(i + length - 1), steps = 0; steps < length / 2; steps++, i = Wrap(i + 1), j = Wrap(j - 1))
            {
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }

            currentPosition = Wrap(currentPosition + length + skipSize);
            skipSize++;
        }

        return (currentPosition, skipSize);

        int Wrap(int input)
        {
            int max = list.Count;
            while (input >= max) input -= max;
            while (input < 0) input += max;
            return input;
        }
    }

    static string CreateDenseHash(IList<int> list)
    {
        var denseHash = new List<string>();

        for (int i = 0; i < 256; i += 16)
        {
            var hash = list[i];
            for (int j = 0; j < 15; j++)
                hash ^= list[i + j + 1];
            denseHash.Add(Convert.ToString(hash, 16).PadLeft(2, '0'));
        }

        return string.Join("", denseHash);
    }
}

