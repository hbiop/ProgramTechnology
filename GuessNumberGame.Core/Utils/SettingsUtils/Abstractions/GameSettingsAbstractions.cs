using GuessNumberGame.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GuessNumberGame.Domain.Utils.SettingsUtils.Abstractions
{
    public interface ISettingsValidator
    {
        IReadOnlyList<string> Validate(GameSettings settings);
    }

    public interface ISettingsSerializer
    {
        string Serialize(GameSettings settings);
        GameSettings? Deserialize(string content);
    }

    public interface ISettingsRepository
    {
        bool Exists { get; }
        string ReadAllText();          
        void WriteAllText(string text); 
    }

    public interface ISettingsService
    {
        GameSettings Current { get; }
        Result Load();
        Result Save(GameSettings settings);
    }
}
