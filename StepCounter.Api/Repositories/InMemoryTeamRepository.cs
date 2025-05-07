using StepCounter.Api.Models;

namespace StepCounter.Api.Repositories;

public class InMemoryTeamRepository : ITeamRepository
{
    private readonly List<Team> _teams = new();

    public IEnumerable<Team> GetAll() => _teams;

    public Team? GetById(Guid id) => _teams.FirstOrDefault(t => t.Id == id);

    public void Add(Team team) => _teams.Add(team);

    public void Remove(Guid id)
    {
        var team = GetById(id);
        if (team != null)
            _teams.Remove(team);
    }

    public void Update(Team team)
    {
        // In-memory update is no-op as we return by reference
    }
}