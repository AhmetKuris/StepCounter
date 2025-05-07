using Moq;
using StepCounter.Api.Models;
using StepCounter.Api.Repositories;
using StepCounter.Api.Services;

namespace StepCounter.Tests.Services;

public class TeamServiceTests
{
    private readonly Mock<ITeamRepository> _repositoryMock;
    private readonly TeamService _service;

    public TeamServiceTests()
    {
        _repositoryMock = new Mock<ITeamRepository>();
        _service = new TeamService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllTeamsAsync_ShouldReturnAllTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new() { Id = Guid.NewGuid(), Name = "Team 1" },
            new() { Id = Guid.NewGuid(), Name = "Team 2" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(teams);

        // Act
        var result = await _service.GetAllTeamsAsync();

        // Assert
        Assert.Equal(teams, result);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTeamAsync_WhenTeamExists_ShouldReturnTeam()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);

        // Act
        var result = await _service.GetTeamAsync(teamId);

        // Assert
        Assert.Equal(team, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
    }

    [Fact]
    public async Task GetTeamAsync_WhenTeamDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync((Team?)null);

        // Act
        var result = await _service.GetTeamAsync(teamId);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
    }

    [Fact]
    public async Task CreateTeamAsync_ShouldCreateAndReturnTeam()
    {
        // Arrange
        var teamName = "New Team";
        var createdTeam = new Team { Id = Guid.NewGuid(), Name = teamName };
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Team>()))
            .ReturnsAsync(createdTeam);

        // Act
        var result = await _service.CreateTeamAsync(teamName);

        // Assert
        Assert.Equal(createdTeam, result);
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Team>(t => t.Name == teamName)), Times.Once);
    }

    [Fact]
    public async Task DeleteTeamAsync_ShouldDeleteTeam()
    {
        // Arrange
        var teamId = Guid.NewGuid();

        // Act
        await _service.DeleteTeamAsync(teamId);

        // Assert
        _repositoryMock.Verify(r => r.RemoveAsync(teamId), Times.Once);
    }

    [Fact]
    public async Task AddCounterAsync_WhenTeamExists_ShouldAddAndReturnCounter()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var counterName = "New Counter";
        var counterId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        var updatedTeam = new Team { Id = teamId, Name = "Test Team" };
        updatedTeam.Counters.Add(new Counter { Id = counterId, Name = counterName });

        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Team>()))
            .Callback<Team>(t =>
            {
                // Ensure the counter in the updated team has the same ID
                var counter = t.Counters.First();
                counter.Id = counterId;
            })
            .ReturnsAsync(updatedTeam);

        // Act
        var result = await _service.AddCounterAsync(teamId, counterName);

        // Assert
        Assert.Equal(counterName, result.Name);
        Assert.Equal(counterId, result.Id);
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Team>()), Times.Once);
    }

    [Fact]
    public async Task AddCounterAsync_WhenTeamDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync((Team?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.AddCounterAsync(teamId, "New Counter"));
    }

    [Fact]
    public async Task RemoveCounterAsync_WhenTeamAndCounterExist_ShouldRemoveCounter()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var counterId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        team.Counters.Add(new Counter { Id = counterId, Name = "Test Counter" });

        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);

        // Act
        await _service.RemoveCounterAsync(teamId, counterId);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Team>()), Times.Once);
    }

    [Fact]
    public async Task RemoveCounterAsync_WhenTeamDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync((Team?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.RemoveCounterAsync(teamId, Guid.NewGuid()));
    }

    [Fact]
    public async Task RemoveCounterAsync_WhenCounterDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.RemoveCounterAsync(teamId, Guid.NewGuid()));
    }

    [Fact]
    public async Task IncrementCounterAsync_WhenTeamAndCounterExist_ShouldIncrementAndReturnCounter()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var counterId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        team.Counters.Add(new Counter { Id = counterId, Name = "Test Counter", Steps = 100 });
        var updatedTeam = new Team { Id = teamId, Name = "Test Team" };
        updatedTeam.Counters.Add(new Counter { Id = counterId, Name = "Test Counter", Steps = 200 });

        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Team>()))
            .ReturnsAsync(updatedTeam);

        // Act
        var result = await _service.IncrementCounterAsync(teamId, counterId, 100);

        // Assert
        Assert.Equal(200, result.Steps);
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Team>()), Times.Once);
    }

    [Fact]
    public async Task IncrementCounterAsync_WhenTeamDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync((Team?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.IncrementCounterAsync(teamId, Guid.NewGuid(), 100));
    }

    [Fact]
    public async Task IncrementCounterAsync_WhenCounterDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.IncrementCounterAsync(teamId, Guid.NewGuid(), 100));
    }

    [Fact]
    public async Task GetTeamTotalStepsAsync_WhenTeamExists_ShouldReturnTotalSteps()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        team.Counters.Add(new Counter { Id = Guid.NewGuid(), Name = "Counter 1", Steps = 100 });
        team.Counters.Add(new Counter { Id = Guid.NewGuid(), Name = "Counter 2", Steps = 200 });

        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);

        // Act
        var result = await _service.GetTeamTotalStepsAsync(teamId);

        // Assert
        Assert.Equal(300, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
    }

    [Fact]
    public async Task GetTeamTotalStepsAsync_WhenTeamDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync((Team?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.GetTeamTotalStepsAsync(teamId));
    }

    [Fact]
    public async Task GetCountersAsync_WhenTeamExists_ShouldReturnCounters()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Test Team" };
        team.Counters.Add(new Counter { Id = Guid.NewGuid(), Name = "Counter 1", Steps = 100 });
        team.Counters.Add(new Counter { Id = Guid.NewGuid(), Name = "Counter 2", Steps = 200 });

        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync(team);

        // Act
        var result = await _service.GetCountersAsync(teamId);

        // Assert
        Assert.Equal(team.Counters, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(teamId), Times.Once);
    }

    [Fact]
    public async Task GetCountersAsync_WhenTeamDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(teamId))
            .ReturnsAsync((Team?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _service.GetCountersAsync(teamId));
    }
} 