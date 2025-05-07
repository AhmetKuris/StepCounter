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

    public IEnumerable<Team> GetAllTeams() => _repository.GetAll();

    public Team? GetTeam(Guid id) => _repository.GetById(id);

    public void CreateTeam(string name)
    {
        _repository.Add(new Team { Name = name });
    }

    public void DeleteTeam(Guid id)
    {
        _repository.Remove(id);
    }

    public void AddCounter(Guid teamId, string name)
    {
        var team = _repository.GetById(teamId);
        if (team == null) return;

        team.Counters.Add(new Counter { Name = name });
    }

    public void RemoveCounter(Guid teamId, Guid counterId)
    {
        var team = _repository.GetById(teamId);
        if (team == null) return;

        var counter = team.Counters.FirstOrDefault(c => c.Id == counterId);
        if (counter != null)
            team.Counters.Remove(counter);
    }

    public void IncrementCounter(Guid teamId, Guid counterId, int steps)
    {
        var team = _repository.GetById(teamId);
        if (team == null) return;

        var counter = team.Counters.FirstOrDefault(c => c.Id == counterId);
        if (counter != null)
            counter.Steps += steps;
    }

    public int GetTeamTotalSteps(Guid teamId)
    {
        var team = _repository.GetById(teamId);
        return team?.Counters.Sum(c => c.Steps) ?? 0;
    }

    public IEnumerable<Counter> GetCounters(Guid teamId)
    {
        var team = _repository.GetById(teamId);
        return team?.Counters ?? Enumerable.Empty<Counter>();
    }
}