namespace Asteroids.Input;

[Flags]
public enum ClientAction
{
    None = 0,
    Accelerate = 1,
    Decelerate = Accelerate << 1,
    Stop = Decelerate << 1,
    TurnLeft = Stop << 1,
    TurnRight = TurnLeft << 1,
    Fire = TurnRight << 1
}