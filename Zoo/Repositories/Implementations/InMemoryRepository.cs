using Zoo.Entities;
using Zoo.Repositories.Interfaces;

namespace Zoo.Repositories.Implementations;

public class InMemoryRepository : IRepository
{
    private readonly List<Keeper> _keepers;
    private readonly List<Aviary> _aviaries;
    private readonly List<Animal> _animals;

    public InMemoryRepository()
    {
        _keepers = new List<Keeper>
        {
            new Keeper(1, "Иванов И.И.", 10),
            new Keeper(2, "Петров П.П.", 3),
            new Keeper(3, "Сидоров С.С.", 7),
            new Keeper(4, "Кузнецов К.К.", 2),
            new Keeper(5, "Смирнов С.С.", 12)
        };

        _aviaries = new List<Aviary>
        {
            new Aviary(1, 7, "Хищники", 100),
            new Aviary(2, 3, "Травоядные", 40),
            new Aviary(3, 5, "Птицы", 25),
            new Aviary(4, 8, "Приматы", 60),
            new Aviary(5, 10, "Ночные животные", 35)
        };

        _animals = new List<Animal>
        {
            new Animal(1, "Лев", 1, 1, "Хищник", 5),
            new Animal(2, "Зебра", 2, 2, "Травоядный", 7),
            new Animal(3, "Тигр", 1, 1, "Хищник", 8),
            new Animal(4, "Жираф", 2, 2, "Травоядный", 8),
            new Animal(5, "Попугай", 3, 3, "Птица", 4)
        };
    }

    public List<Keeper> GetKeepers() => new List<Keeper>(_keepers);
    public List<Aviary> GetAviaries() => new List<Aviary>(_aviaries);
    public List<Animal> GetAnimals() => new List<Animal>(_animals);
}
