using StepCounter.Api.Models;

namespace StepCounter.Api.Repositories;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetAllAsync();
    Task<Team?> GetByIdAsync(Guid id);
    Task<Team> AddAsync(Team team);
    Task RemoveAsync(Guid id);
    Task<Team> UpdateAsync(Team team);
}
