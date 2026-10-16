using Bluda_V_Restorane.enums;

namespace Bluda_V_Restorane.classes;


/// <summary>
/// Базовый класс для всех блюд в ресторане
/// </summary>
public abstract class Dish
{
    private int _id;

    public string Name { get; set; }
    public decimal Price { get; set; }

    public Course Course { get; set; } = Course.Main;

    public decimal PriceWithService => Price * 1.1m;

    protected Dish(int id, string name, decimal price)
    {
        _id = id;
        Name = name;
        Price = price;
    }

    protected Dish(string name) : this(0, name, 0)
    {
    }

    public abstract void Cook();
}