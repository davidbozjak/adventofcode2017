using System.Text.RegularExpressions;

var particles = new InputProvider<Particle?>("Input.txt", GetParticle).Where(w => w != null).Cast<Particle>().ToList();

for (int id = 0; id < particles.Count; id++)
    particles[id].Id = id.ToString();

var intedeterminate = particles.Where(w => w.IsIndeterminte()).ToList();

while (intedeterminate.Count > 0)
{
    particles.ForEach(w => w.MakeStep());
    intedeterminate = intedeterminate.Where(w => w.IsIndeterminte()).ToList();
}

var closest = particles
    .Select(w => new { Id = w.Id, Distance = Math.Abs(w.PositionX) + Math.Abs(w.PositionY) + Math.Abs(w.PositionZ) })
    .OrderBy(w => w.Distance)
    .First();

Console.WriteLine($"Part 1: {closest.Id}");

static bool GetParticle(string? input, out Particle? value)
{
    value = null;

    if (input == null) return false;

    Regex numRegex = new(@"-?\d+");

    value = new Particle(numRegex.Matches(input).Select(w => int.Parse(w.Value)).ToArray());

    return true;
}

class Particle
{
    public string Id { get; set;  }

    public long PositionX { get; private set; }
    public long PositionY { get; private set; }
    public long PositionZ { get; private set; }
           
    public long VelocityX { get; private set; }
    public long VelocityY { get; private set; }
    public long VelocityZ { get; private set; }
           
    public long AccelerationX { get; private set; }
    public long AccelerationY { get; private set; }
    public long AccelerationZ { get; private set; }

    public Particle(int[] initialValues)
    {
        if (initialValues.Length != 9) throw new Exception();

        this.PositionX = initialValues[0];
        this.PositionY = initialValues[1];
        this.PositionZ = initialValues[2];

        this.VelocityX = initialValues[3];
        this.VelocityY = initialValues[4];
        this.VelocityZ = initialValues[5];

        this.AccelerationX = initialValues[6];
        this.AccelerationY = initialValues[7];
        this.AccelerationZ = initialValues[8];
    }

    public void MakeStep()
    {
        this.VelocityX += this.AccelerationX;
        this.VelocityY += this.AccelerationY;
        this.VelocityZ += this.AccelerationZ;

        this.PositionX += this.VelocityX;
        this.PositionY += this.VelocityY;
        this.PositionZ += this.VelocityZ;
    }

    public bool IsIndeterminte()
    {
        if ((this.AccelerationX != 0) && (IsPositive(this.AccelerationX) != IsPositive(this.VelocityX))) return true;
        if ((this.VelocityX != 0) && (IsPositive(this.VelocityX) != IsPositive(this.PositionX))) return true;

        if ((this.AccelerationY != 0) && (IsPositive(this.AccelerationY) != IsPositive(this.VelocityY))) return true;
        if ((this.VelocityY != 0) && (IsPositive(this.VelocityY) != IsPositive(this.PositionY))) return true;

        if ((this.AccelerationZ != 0) && (IsPositive(this.AccelerationZ) != IsPositive(this.VelocityZ))) return true;
        if ((this.VelocityZ != 0) && (IsPositive(this.VelocityZ) != IsPositive(this.PositionZ))) return true;

        return false;

        bool IsPositive(long x) => x > 0;
    }
}