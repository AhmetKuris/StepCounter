using StepCounter.Api.Models;

namespace StepCounter.Api.Repositories;

public interface ITeamRepository
{
    IEnumerable<Team> GetAll();
    Team? GetById(Guid id);
    void Add(Team team);
    void Remove(Guid id);
    void Update(Team team);
}
