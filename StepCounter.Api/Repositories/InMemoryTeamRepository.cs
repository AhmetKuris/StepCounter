using StepCounter.Api.Models;

namespace StepCounter.Api.Repositories;

public class InMemoryTeamRepository : ITeamRepository
{
    private readonly List<Team> _teams = new();

    public Task<IEnumerable<Team>> GetAllAsync() => Task.FromResult(_teams.AsEnumerable());

    public Task<Team?> GetByIdAsync(Guid id) => Task.FromResult(_teams.FirstOrDefault(t => t.Id == id));

    public Task<Team> AddAsync(Team team)
    {
        _teams.Add(team);
        return Task.FromResult(team);
    }

    public Task RemoveAsync(Guid id)
    {
        var team = _teams.FirstOrDefault(t => t.Id == id);
        if (team != null)
            _teams.Remove(team);
        return Task.CompletedTask;
    }

    public Task<Team> UpdateAsync(Team team)
    {
        // In-memory update is no-op as we return by reference
        return Task.FromResult(team);
    }
}