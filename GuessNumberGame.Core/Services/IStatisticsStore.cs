using GuessNumberGame.Domain.Models;

namespace GuessNumberGame.Domain.Services;

public interface IStatisticsStore
{
    PlayerStatistics Load();

    void Save(PlayerStatistics statistics);
}
