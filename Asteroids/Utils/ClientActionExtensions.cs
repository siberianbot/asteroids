using Asteroids.Input;

namespace Asteroids.Utils;

public static class ClientActionExtensions
{
    public static ClientAction EnableAction(this ClientAction currentAction, ClientAction requiredAction)
    {
        return currentAction | requiredAction;
    }

    public static ClientAction DisableAction(this ClientAction currentAction, ClientAction requiredAction)
    {
        return currentAction & ~requiredAction;
    }
}