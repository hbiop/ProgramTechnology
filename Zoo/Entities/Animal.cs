namespace Zoo.Entities;

public class Animal
{
    public int Id { get; }
    public string Name { get; }
    public int KeeperId { get; }
    public int AviaryId { get; }
    public string Species { get; }
    public int Age { get; }

    public bool IsPredator => Species == "Хищник";
    public bool IsOld => Age > 10;

    public Animal(int id, string name, int keeperId, int aviaryId, string species, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя животного не может быть пустым.", nameof(name));
        if (string.IsNullOrWhiteSpace(species))
            throw new ArgumentException("Вид животного не может быть пустым.", nameof(species));
        if (age < 0)
            throw new ArgumentOutOfRangeException(nameof(age), "Возраст не может быть отрицательным.");

        Id = id;
        Name = name;
        KeeperId = keeperId;
        AviaryId = aviaryId;
        Species = species;
        Age = age;
    }

    public string GetInfo() => $"{Name} ({Age} лет, {Species.ToLowerInvariant()})";
}
