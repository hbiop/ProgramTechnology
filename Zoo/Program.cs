using Zoo.Entities;
using Zoo.Repositories.Implementations;
using Zoo.Repositories.Interfaces;
using Zoo.Services;

namespace Zoo;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("aaaaa");
        Console.WriteLine("Выберите источник данных:");
        Console.WriteLine("1 — InMemoryRepository");
        Console.WriteLine("2 — CsvRepository");
        Console.Write("Ваш выбор: ");

        if (!int.TryParse(Console.ReadLine(), out var choice))
        {
            Console.WriteLine("Неверный выбор");
            return;
        }

        IRepository repository;
        switch (choice)
        {
            case 1:
                repository = new InMemoryRepository();
                break;
            case 2:
                repository = new CsvRepository("data");
                break;
            default:
                Console.WriteLine("Неверный выбор");
                return;
        }

        PrintExample(new ZooService(repository), repository);
    }

    private static void PrintExample(ZooService service, IRepository repository)
    {
        var animals = repository.GetAnimals();
        Animal? lion = null;
        foreach (var animal in animals)
        {
            if (animal.Name == "Лев")
            {
                lion = animal;
                break;
            }
        }

        Console.WriteLine($"1. FindKeeper(\"Лев\"): {service.FindKeeper("Лев")?.GetInfo() ?? "null"}");
        Console.WriteLine($"2. FindAviary(animal \"Лев\"): "
            + (lion is null ? "null" : service.FindAviary(lion)?.GetInfo() ?? "null"));
        Console.WriteLine($"3. GetAverageAge: {service.GetAverageAge()} лет");

        Console.Write("4. CountAnimalsBySpecies: ");
        var counts = service.CountAnimalsBySpecies();
        var first = true;
        foreach (var item in counts)
        {
            if (!first)
                Console.Write(", ");
            Console.Write($"{item.Key} — {item.Value}");
            first = false;
        }
        Console.WriteLine();

        Console.WriteLine("5. PrintAllAnimals:");
        Console.WriteLine(service.PrintAllAnimals());
        Console.WriteLine();
        Console.WriteLine($"Не найдено: FindKeeper(\"Неизвестное животное\") → "
            + (service.FindKeeper("Неизвестное животное")?.GetInfo() ?? "null"));
    }
}
