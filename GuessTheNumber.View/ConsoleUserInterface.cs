namespace GuessTheNumber.View;

public sealed class ConsoleUserInterface : IUserInterface
{
    public void Write(string message) => Console.Write(message);

    public void WriteLine(string message = "") => Console.WriteLine(message);

    public string? ReadLine(string prompt)
    {
        Write(prompt);
        return Console.ReadLine();
    }
}
