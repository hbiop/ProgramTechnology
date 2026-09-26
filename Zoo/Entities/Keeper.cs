namespace Zoo.Entities;

public class Keeper
{
    public int Id { get; }
    public string FullName { get; }
    public int Experience { get; }

    public bool IsExperienced => Experience > 5;

    public Keeper(int id, string fullName, int experience)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("ФИО смотрителя не может быть пустым.", nameof(fullName));
        if (experience < 0)
            throw new ArgumentOutOfRangeException(nameof(experience), "Опыт не может быть отрицательным.");

        Id = id;
        FullName = fullName;
        Experience = experience;
    }

    public string GetInfo() => $"{FullName} ({Experience} лет опыта)";
}
