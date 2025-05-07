using StepCounter.Api.Models;
using StepCounter.Api.Repositories;

namespace StepCounter.Api.Services;

public class TeamService
{
    private readonly ITeamRepository _repository;

    public TeamService(ITeamRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Team>> GetAllTeamsAsync() => await _repository.GetAllAsync();

    public async Task<Team?> GetTeamAsync(Guid id) => await _repository.GetByIdAsync(id);

    public async Task<Team> CreateTeamAsync(string name)
    {
        var team = new Team { Name = name };
        return await _repository.AddAsync(team);
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        await _repository.RemoveAsync(id);
    }

    public async Task<Counter> AddCounterAsync(Guid teamId, string name)
    {
        var team = await _repository.GetByIdAsync(teamId);
        if (team == null) throw new KeyNotFoundException($"Team with ID {teamId} not found");

        var counter = new Counter { Name = name };
        team.Counters.Add(counter);
        var updatedTeam = await _repository.UpdateAsync(team);
        
        var addedCounter = updatedTeam.Counters.FirstOrDefault(c => c.Name == name);
        if (addedCounter == null)
            throw new InvalidOperationException($"Failed to add counter '{name}' to team {teamId}. The counter was not found in the updated team.");

        return addedCounter;
    }

    public async Task RemoveCounterAsync(Guid teamId, Guid counterId)
    {
        var team = await _repository.GetByIdAsync(teamId);
        if (team == null) throw new KeyNotFoundException($"Team with ID {teamId} not found");

        var counter = team.Counters.FirstOrDefault(c => c.Id == counterId);
        if (counter == null) throw new KeyNotFoundException($"Counter with ID {counterId} not found");

        team.Counters.Remove(counter);
        await _repository.UpdateAsync(team);
    }

    public async Task<Counter> IncrementCounterAsync(Guid teamId, Guid counterId, int steps)
    {
        var team = await _repository.GetByIdAsync(teamId);
        if (team == null) throw new KeyNotFoundException($"Team with ID {teamId} not found");

        var counter = team.Counters.FirstOrDefault(c => c.Id == counterId);
        if (counter == null) throw new KeyNotFoundException($"Counter with ID {counterId} not found");

        counter.Steps += steps;
        var updatedTeam = await _repository.UpdateAsync(team);
        
        var updatedCounter = updatedTeam.Counters.FirstOrDefault(c => c.Id == counterId);
        if (updatedCounter == null)
            throw new InvalidOperationException($"Failed to update counter {counterId} in team {teamId}. The counter was not found in the updated team.");

        return updatedCounter;
    }

    public async Task<int> GetTeamTotalStepsAsync(Guid teamId)
    {
        var team = await _repository.GetByIdAsync(teamId);
        if (team == null) throw new KeyNotFoundException($"Team with ID {teamId} not found");
        return team.Counters.Sum(c => c.Steps);
    }

    public async Task<IEnumerable<Counter>> GetCountersAsync(Guid teamId)
    {
        var team = await _repository.GetByIdAsync(teamId);
        if (team == null) throw new KeyNotFoundException($"Team with ID {teamId} not found");
        return team.Counters;
    }
}