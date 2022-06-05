using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Silk.NET.Input;

namespace Asteroids.Behaviors;

public class PlayerControlBehavior : IBehavior
{
    [Flags]
    private enum Action
    {
        None = 0,
        Accelerate = 0b1,
        Decelerate = 0b10,
        Stop = 0b100,
        TurnLeft = 0b1000,
        TurnRight = 0b10000,
        Fire = 0b100000
    }

    private readonly PlayerController _playerController;
    private readonly EntityController _entityController;
    private readonly EventQueue _eventQueue;
    private long _keyPressSubscription;
    private long _keyReleaseSubscription;
    private Action _currentAction = Action.None;

    public PlayerControlBehavior(PlayerController playerController, EntityController entityController, EventQueue eventQueue)
    {
        _playerController = playerController;
        _entityController = entityController;
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _keyPressSubscription = _eventQueue.Subscribe(EventType.KeyPress, @event => HandleKeyPress(@event.Key!.Value));
        _keyReleaseSubscription = _eventQueue.Subscribe(EventType.KeyRelease, @event => HandleKeyRelease(@event.Key!.Value));
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.KeyPress, _keyPressSubscription);
        _eventQueue.Unsubscribe(EventType.KeyRelease, _keyReleaseSubscription);
    }

    public void Update(float delta)
    {
        foreach (Player player in _playerController.Players)
        {
            if (!player.Alive)
            {
                continue;
            }

            Spaceship? ownedSpaceship = _entityController.GetOwnedEntities<Spaceship>(player).SingleOrDefault();

            if (ownedSpaceship == null)
            {
                continue;
            }

            SpaceshipControlComponent spaceshipControlComponent = ownedSpaceship.GetComponent<SpaceshipControlComponent>()
                                                                  ?? throw new ArgumentException();
            BulletSpawnerComponent bulletSpawnerComponent = ownedSpaceship.GetComponent<BulletSpawnerComponent>()
                                                            ?? throw new ArgumentException();

            if (_currentAction.HasFlag(Action.TurnLeft))
            {
                spaceshipControlComponent.TurnLeft();
            }

            if (_currentAction.HasFlag(Action.TurnRight))
            {
                spaceshipControlComponent.TurnRight();
            }

            if (_currentAction.HasFlag(Action.Stop))
            {
                spaceshipControlComponent.Stop();
            }

            if (_currentAction.HasFlag(Action.Accelerate))
            {
                spaceshipControlComponent.Accelerate();
            }

            if (_currentAction.HasFlag(Action.Decelerate))
            {
                spaceshipControlComponent.Decelerate();
            }

            if (_currentAction.HasFlag(Action.Fire))
            {
                bulletSpawnerComponent.Fire();
            }
        }
    }

    private void HandleKeyPress(Key key)
    {
        _currentAction |= KeyToAction(key);
    }

    private void HandleKeyRelease(Key key)
    {
        _currentAction &= ~KeyToAction(key);
    }

    private static Action KeyToAction(Key key)
    {
        return key switch
        {
            Key.Up => Action.Accelerate,
            Key.Down => Action.Decelerate,
            Key.Z => Action.Stop,
            Key.Left => Action.TurnLeft,
            Key.Right => Action.TurnRight,
            Key.Space => Action.Fire,
            _ => Action.None
        };
    }
}