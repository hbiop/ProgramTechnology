using Zoo.Entities;
using Zoo.Repositories.Interfaces;

namespace Zoo.Services;

public class ZooService
{
    private readonly IRepository _repository;

    public ZooService(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Keeper? FindKeeper(Animal animal)
    {
        foreach (var keeper in _repository.GetKeepers())
        {
            if (keeper.Id == animal.KeeperId)
                return keeper;
        }

        return null;
    }

    public Keeper? FindKeeper(string animalName)
    {
        foreach (var animal in _repository.GetAnimals())
        {
            if (animal.Name == animalName)
                return FindKeeper(animal);
        }

        return null;
    }

    public Aviary? FindAviary(Animal animal)
    {
        foreach (var aviary in _repository.GetAviaries())
        {
            if (aviary.Id == animal.AviaryId)
                return aviary;
        }

        return null;
    }

    public Aviary? FindAviary(string animalName)
    {
        foreach (var animal in _repository.GetAnimals())
        {
            if (animal.Name == animalName)
                return FindAviary(animal);
        }

        return null;
    }

    public int GetAverageAge()
    {
        var animals = _repository.GetAnimals();
        if (animals.Count == 0)
            return 0;

        var totalAge = 0;
        foreach (var animal in animals)
            totalAge += animal.Age;

        return (int)Math.Round((double)totalAge / animals.Count, MidpointRounding.AwayFromZero);
    }

    public Dictionary<string, int> CountAnimalsBySpecies()
    {
        var result = new Dictionary<string, int>();

        foreach (var animal in _repository.GetAnimals())
        {
            if (result.ContainsKey(animal.Species))
                result[animal.Species]++;
            else
                result.Add(animal.Species, 1);
        }

        return result;
    }

    public string PrintAllAnimals()
    {
        var result = new List<string>();

        foreach (var animal in _repository.GetAnimals())
        {
            var keeper = FindKeeper(animal);
            var aviary = FindAviary(animal);
            var keeperName = keeper is null ? "—" : keeper.FullName;
            var aviaryNumber = aviary is null ? "—" : $"№{aviary.Number}";
            result.Add($"\"{animal.GetInfo()}\" — смотритель {keeperName}, вольер {aviaryNumber}");
        }

        return string.Join(Environment.NewLine, result);
    }
}
