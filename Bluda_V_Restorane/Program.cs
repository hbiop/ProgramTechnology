using Bluda_V_Restorane.classes;
using Bluda_V_Restorane.enums;
using Bluda_V_Restorane.interfaces;

namespace Bluda_V_Restorane;

using System;

public class Program
{
    public static void Main()
    {
        Soup soup = new Soup(1, "Borscht", 350)
        {
            IsVegetarian = false,
            Course = Course.Main
        };

        Steak steak = new Steak(2, "Ribeye", 1500)
        {
            Doneness = Doneness.Medium,
            Course = Course.Main
        };

        PrintDish(soup);
        PrintDish(steak);
    }

    private static void PrintDish(Dish dish)
    {
        Console.Write($"[{dish.GetType().Name}] {dish.Name}, {dish.Price}, course: {dish.Course}");

        if (dish is Steak steak)
        {
            Console.Write($", doneness: {steak.Doneness.ToString().ToLowerInvariant()}");
        }

        Console.WriteLine();
        Console.WriteLine($"  price with service: {dish.PriceWithService:0.##}");

        dish.Cook();

        if (dish is IServable servable)
        {
            servable.Serve();
        }
        else
        {
            Console.WriteLine($"{dish.Name} cannot be served.");
        }

        Console.WriteLine("----------------------------------------");
    }
}
