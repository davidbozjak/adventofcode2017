using System.Drawing;

bool animate = false;

var lines = new InputProvider<string>("Input.txt", GetString).ToList();

var tracks = new List<Track>();

for(int y = 0; y < lines.Count; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] == ' ') continue;

        tracks.Add(new Track(x, y, lines[y][x]));
    }
}

var world = new TrackWorld(tracks);
var train = new Train();
world.Train = train;
train.ResetPosition(world.Tracks.First(w => w.Position.Y == 0), Train.Heading.Down);

tracks.ForEach(w => w.SetNeighbours(tracks));

var worldPrinter = new WorldPrinter(skipEmptyLines: false);

while (train.MakeStep())
{
    if (animate)
    {
        worldPrinter.Print(world, train);
        Thread.Sleep(200);
    }
}

Console.WriteLine($"Part 1: Path: {train.VisitedCheckpoints}");
Console.WriteLine($"Part 1: StepsTaken: {train.StepsTaken}");


static bool GetString(string? input, out string? value)
{
    value = null;

    if (input == null) return false;

    value = input ?? string.Empty;

    return true;
}

class TrackWorld : IWorld
{
    public IEnumerable<IWorldObject> WorldObjects => this.tracks.Cast<IWorldObject>().Append(this.Train);

    private readonly List<Track> tracks = new List<Track>();
    public IEnumerable<Track> Tracks => this.tracks;

    public TrackWorld(IEnumerable<Track> tracks)
    {
        this.tracks.AddRange(tracks);
    }

    public Train Train { get; set; }
}

class Track : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation { get; }

    public int Z => 0;

    public Track (int x, int y, char type)
    {
        this.Position = new Point(x, y);
        this.CharRepresentation = type;
    }

    public IEnumerable<Track> Neighbours => 
        new[] { this.NeighbourBelow, this.NeighbourAbove, this.NeighbourLeft, this.NeighbourRight }
        .Where(w => w != null)
        .Cast<Track>();

    public void SetNeighbours(IEnumerable<Track> tracks)
    {
        if (this.CharRepresentation == ' ')
            throw new Exception();

        this.NeighbourBelow = tracks.FirstOrDefault(w => w.Position.X == this.Position.X && w.Position.Y == this.Position.Y + 1);
        this.NeighbourAbove = tracks.FirstOrDefault(w => w.Position.X == this.Position.X && w.Position.Y == this.Position.Y - 1);
        this.NeighbourLeft =  tracks.FirstOrDefault(w => w.Position.X == this.Position.X - 1 && w.Position.Y == this.Position.Y);
        this.NeighbourRight = tracks.FirstOrDefault(w => w.Position.X == this.Position.X + 1 && w.Position.Y == this.Position.Y);
    }

    public Track? NeighbourBelow { get; private set; }
    public Track? NeighbourAbove { get; private set; }
    public Track? NeighbourLeft { get; private set; }
    public Track? NeighbourRight { get; private set; }
}

class Train : IWorldObject
{
    public enum Heading { Up, Down, Left, Right };
    private Heading heading;

    public Point Position => this.Location.Position;

    public char CharRepresentation => 'x';

    public Track Location { get; private set; }

    private readonly List<Track> visitedTracks = new List<Track>();
    public string VisitedCheckpoints => new string(this.visitedTracks.Where(w => char.IsLetter(w.CharRepresentation)).Select(w => w.CharRepresentation).ToArray());

    public int StepsTaken => this.visitedTracks.Count;

    public int Z => 1;

    public void ResetPosition(Track track, Heading heading)
    {
        this.visitedTracks.Clear();
        this.heading = heading;
        this.MoveToTrack(track);
    }

    public bool MakeStep()
    {
        bool needToTurn = false;

        if (this.heading == Heading.Down)
        {
            var trackBelow = this.Location.NeighbourBelow;
            if (trackBelow == null)
            {
                needToTurn = true;
            }
            else
            {
                MoveToTrack(trackBelow);
                return true;
            }
        }
        else if (this.heading == Heading.Up)
        {
            var trackAbove = this.Location.NeighbourAbove;
            if (trackAbove == null)
            {
                needToTurn = true;
            }
            else
            {
                MoveToTrack(trackAbove);
                return true;
            }
        }
        else if (this.heading == Heading.Left)
        {
            var trackLeft = this.Location.NeighbourLeft;
            if (trackLeft == null)
            {
                needToTurn = true;
            }
            else
            {
                MoveToTrack(trackLeft);
                return true;
            }
        }
        else if (this.heading == Heading.Right)
        {
            var trackRight = this.Location.NeighbourRight;
            if (trackRight == null)
            {
                needToTurn = true;
            }
            else
            {
                MoveToTrack(trackRight);
                return true;
            }
        }

        if (needToTurn)
        {
            if (this.Location.Neighbours.Count() < 2)
                return false;
            if (this.Location.Neighbours.Count() > 2) 
                throw new Exception();

            var track = this.Location.Neighbours.First(w => !visitedTracks.Contains(w));
            
            this.heading = (track.Position.X - this.Position.X, track.Position.Y - this.Position.Y) switch
            {
                (0, -1) => Heading.Up,
                (0, 1) => Heading.Down,
                (-1, 0) => Heading.Left,
                (1, 0) => Heading.Right,
                _ => throw new Exception()
            };

            MoveToTrack(track);
            return true;
        }

        return false;
    }

    private void MoveToTrack(Track track)
    {
        if (this.Location == track)
            throw new Exception();
        
        this.Location = track;
        this.visitedTracks.Add(track);
    }
}
