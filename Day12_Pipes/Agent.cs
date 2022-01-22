using System.Diagnostics;

[DebuggerDisplay("Id: {Id}")]
class Agent
{
    public int Id { get; }

    private readonly List<Agent> reachableAgents = new();
    public IReadOnlyCollection<Agent> ReachableAgents => this.reachableAgents.AsReadOnly();

    public Agent(int id)
    {
        this.Id = id;
    }

    public void AddAgents(IEnumerable<Agent> connections)
        => this.reachableAgents.AddRange(connections);


    public HashSet<Agent> GetAllReachableAgents()
    {
        var reachable = new HashSet<Agent>();

        PopulateAllReachable(reachable);

        return reachable;
    }

    private void PopulateAllReachable(HashSet<Agent> reachable)
    {
        if (reachable.Contains(this)) return;
        reachable.Add(this);

        foreach (var agent in reachableAgents)
        {
            agent.PopulateAllReachable(reachable);
        }
    }

    public bool CanReach(Agent agent)
    {
        return CanReach(agent, new HashSet<Agent>());
    }

    private bool CanReach(Agent agent, HashSet<Agent> visited)
    {
        if (visited.Contains(this)) return false;
        visited.Add(this);

        if (agent == this) return true;

        foreach (var reachableAgent in this.ReachableAgents)
            if (reachableAgent.CanReach(agent, visited)) return true;

        return false;
    }
}