using Zoo.Entities;

namespace Zoo.Repositories.Interfaces;

public interface IRepository
{
    List<Keeper> GetKeepers();
    List<Aviary> GetAviaries();
    List<Animal> GetAnimals();
}
