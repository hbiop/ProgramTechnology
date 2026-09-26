using System.Globalization;
using Zoo.Entities;
using Zoo.Repositories.Interfaces;

namespace Zoo.Repositories.Implementations;

public class CsvRepository : IRepository
{

    private readonly List<Keeper> _keepers = new List<Keeper>();
    private readonly List<Aviary> _aviaries = new List<Aviary>();
    private readonly List<Animal> _animals = new List<Animal>();

    public CsvRepository(string dataDirectory)
    {
        if (string.IsNullOrWhiteSpace(dataDirectory))
            throw new ArgumentException("Папка с данными не указана.", nameof(dataDirectory));

        LoadKeepers(Path.Combine(dataDirectory, "keepers.csv"));
        LoadAviaries(Path.Combine(dataDirectory, "aviaries.csv"));
        LoadAnimals(Path.Combine(dataDirectory, "animals.csv"));
        ValidateForeignKeys();
    }

    public List<Keeper> GetKeepers() => new List<Keeper>(_keepers);
    public List<Aviary> GetAviaries() => new List<Aviary>(_aviaries);
    public List<Animal> GetAnimals() => new List<Animal>(_animals);

    private void LoadKeepers(string path)
    {
        var lines = ReadFile(path, "id,fullName,experience");
        for (var i = 1; i < lines.Length; i++)
        {
            var values = SplitLine(lines[i], 3, path, i + 1);
            _keepers.Add(new Keeper(
                int.Parse(values[0]),
                values[1],
                int.Parse(values[2])));
        }
    }

    private void LoadAviaries(string path)
    {
        var lines = ReadFile(path, "id,number,type,area");
        for (var i = 1; i < lines.Length; i++)
        {
            var values = SplitLine(lines[i], 4, path, i + 1);
            var aviary = new Aviary(
                int.Parse(values[0]),
                int.Parse(values[1]),
                values[2],
                double.Parse(values[3], CultureInfo.InvariantCulture));

            foreach (var existing in _aviaries)
            {
                if (existing.Number == aviary.Number)
                    throw new InvalidDataException($"Повторяющийся номер вольера: {aviary.Number}.");
            }

            _aviaries.Add(aviary);
        }
    }

    private void LoadAnimals(string path)
    {
        var lines = ReadFile(path, "id,name,keeperId,aviaryId,species,age");
        for (var i = 1; i < lines.Length; i++)
        {
            var values = SplitLine(lines[i], 6, path, i + 1);
            _animals.Add(new Animal(
                int.Parse(values[0]),
                values[1],
                int.Parse(values[2]),
                int.Parse(values[3]),
                values[4],
                int.Parse(values[5])));
        }
    }

    private static string[] ReadFile(string path, string expectedHeader)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"CSV-файл не найден: {path}", path);

        var lines = File.ReadAllLines(path);
        if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
            throw new InvalidDataException($"CSV-файл пуст: {path}");
        if (lines[0].Trim() != expectedHeader)
            throw new InvalidDataException($"Некорректный заголовок CSV-файла: {path}");

        return lines;
    }

    private static string[] SplitLine(string line, int expectedCount, string path, int lineNumber)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new InvalidDataException($"Пустая строка в {path}, строка {lineNumber}.");

        var values = line.Split(',');
        if (values.Length != expectedCount)
            throw new InvalidDataException($"Некорректное число полей в {path}, строка {lineNumber}.");

        for (var i = 0; i < values.Length; i++)
            values[i] = values[i].Trim();

        return values;
    }

    private void ValidateForeignKeys()
    {
        foreach (var animal in _animals)
        {
            if (!HasKeeper(animal.KeeperId) || !HasAviary(animal.AviaryId))
                throw new InvalidDataException($"Для животного {animal.Name} не найден внешний ключ.");
        }
    }

    private bool HasKeeper(int id)
    {
        foreach (var keeper in _keepers)
            if (keeper.Id == id) return true;
        return false;
    }

    private bool HasAviary(int id)
    {
        foreach (var aviary in _aviaries)
            if (aviary.Id == id) return true;
        return false;
    }
}
