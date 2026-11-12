using GuessNumberGame.Domain.Models;

namespace GuessNumberGame.Domain.Services;

public interface ISettingsStore
{
    GameSettings Load();

    void Save(GameSettings settings);
}
