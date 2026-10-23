using Bluda_V_Restorane.interfaces;

namespace Bluda_V_Restorane.classes;

public class Soup : Dish, IServable
{
    public bool IsVegetarian { get; init; }

    public bool IsLight => IsVegetarian;

    public Soup(int id, string name, decimal price)
        : base(id, name, price)
    {
    }

    public Soup(string name)
        : base(name)
    {
    }

    public override void Cook()
    {
        Console.WriteLine($"{Name} cooked.");
    }

    public void Serve()
    {
        Console.WriteLine($"{Name} served.");
    }
}
