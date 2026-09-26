namespace Zoo.Entities;

public class Aviary
{
    public int Id { get; }
    public int Number { get; }
    public string Type { get; }
    public double Area { get; }

    public bool IsBig => Area > 50;

    public Aviary(int id, int number, string type, double area)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Тип вольера не может быть пустым.", nameof(type));
        if (area < 0)
            throw new ArgumentOutOfRangeException(nameof(area), "Площадь не может быть отрицательной.");

        Id = id;
        Number = number;
        Type = type;
        Area = area;
    }

    public string GetInfo() => $"Вольер №{Number} ({Area} м², {Type.ToLowerInvariant()})";
}
