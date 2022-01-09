var passphrases = new InputProvider<string[]?>("Input.txt", GetPassPhrases).Where(w => w != null).Cast<string[]>().ToList();

Console.WriteLine($"Part 1: {CountEntriesWithOnlyUniqueWords(passphrases)}");

var passphraseAnagrams = passphrases
    .Select(w => 
        w.Select(ww => 
            string.Join(',', ww.ToCharArray().GroupBy(www => www).OrderBy(w => w.Key).Select(www => $"(({(www.Key)}):({www.Count()}))")))
    );

Console.WriteLine($"Part 2: {CountEntriesWithOnlyUniqueWords(passphraseAnagrams)}");

static bool GetPassPhrases(string? input, out string[]? value)
{
    value = null;

    if (input == null) return false;

    value = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    return true;
}

static int CountEntriesWithOnlyUniqueWords(IEnumerable<IEnumerable<string>> entries)
{
    var dict = entries.Select(w => w.GroupBy(w => w).ToDictionary(w => w.Key, w => w.Count()));

    var countUnique = dict.Count(w => w.Values.Max() == 1);

    return countUnique;
}