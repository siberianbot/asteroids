namespace Asteroids.Engine;

public class CommandQueue
{
    public delegate void Command();

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