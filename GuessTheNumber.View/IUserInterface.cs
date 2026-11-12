namespace GuessTheNumber.View;

public interface IUserInterface
{
    void Write(string message);

    void WriteLine(string message = "");

    string? ReadLine(string prompt);
}
