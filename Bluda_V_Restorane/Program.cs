namespace Bluda_V_Restorane;

using System;
using Bluda_V_Restorane.classes;
using Bluda_V_Restorane.enums;
using Bluda_V_Restorane.interfaces;


public class Program
{
    public static void Main()
    {
        Dish[] dishes =
        {
            new Soup(1, "Borscht", 350)
            {
                IsVegetarian = false,
                Course = Course.Main
            },

            new Steak(2, "Ribeye", 1500)
            {
                Doneness = Doneness.Medium,
                Course = Course.Main
            },

            new Cake(3, "Napoleon", 800)
            {
                Layers = 5,
                Course = Course.Dessert
            }
        };

        foreach (Dish dish in dishes)
        {
            PrintDish(dish);
        }
    }

    private static void PrintDish(Dish dish)
    {
        Console.Write($"[{dish.GetType().Name}] ");
        Console.Write($"{dish.Name}, {dish.Price:0.##}, course: {dish.Course}");

        if (dish is Steak steak)
        {
            Console.Write($", doneness: ");
            Console.Write(steak.Doneness.ToString().ToLowerInvariant());
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