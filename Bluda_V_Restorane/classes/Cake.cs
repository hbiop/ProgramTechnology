using Bluda_V_Restorane.interfaces;

namespace Bluda_V_Restorane.classes;

public class Cake : Dish, IServable
{
    public int Layers { get; init; }

    public bool IsBigCake => Layers >= 3;

    public Cake(int id, string name, decimal price)
        : base(id, name, price)
    {
    }

    public Cake(string name)
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
