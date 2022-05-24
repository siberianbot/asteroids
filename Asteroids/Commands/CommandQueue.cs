namespace Asteroids.Commands;

public delegate void Command();

public class CommandQueue
{
    private List<Command> _pendingCommands = new List<Command>();

    public void Push(Command command)
    {
        _pendingCommands.Add(command);
    }

    public void ExecutePending()
    {
        List<Command> commands = _pendingCommands;

        _pendingCommands = new List<Command>();

        foreach (Command command in commands)
        {
            command();
        }
    }
}