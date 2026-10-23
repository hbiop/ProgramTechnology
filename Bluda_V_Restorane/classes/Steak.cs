using Bluda_V_Restorane.enums;

namespace Bluda_V_Restorane.classes;

public class Steak : Dish
{
    public Doneness Doneness { get; init; } = Doneness.Medium;

    public bool IsRare => Doneness == Doneness.Rare;

    public Steak(int id, string name, decimal price)
        : base(id, name, price)
    {
    }

    public Steak(string name)
        : base(name)
    {
    }

    public override void Cook()
    {
        Console.WriteLine($"{Name} cooked.");
    }
}
