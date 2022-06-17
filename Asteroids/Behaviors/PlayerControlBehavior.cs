using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Input;
using Asteroids.Utils;

namespace Asteroids.Behaviors;

public class PlayerControlBehavior : IBehavior
{
    private readonly Dictionary<Player, ClientAction> _currentActions = new Dictionary<Player, ClientAction>();
    private readonly PlayerController _playerController;
    private readonly EntityController _entityController;
    private readonly EventQueue _eventQueue;
    private long _clientActionSubscription;

    public PlayerControlBehavior(PlayerController playerController, EntityController entityController, EventQueue eventQueue)
    {
        _playerController = playerController;
        _entityController = entityController;
        _eventQueue = eventQueue;
    }

    public void Initialize()
    {
        _clientActionSubscription = _eventQueue.Subscribe(EventType.ClientAction, @event =>
        {
            if (@event.ClientAction == null)
            {
                return;
            }

            Player? player = @event.Client?.Player;

            if (player == null)
            {
                return;
            }

            if (!_currentActions.TryGetValue(player, out ClientAction currentAction))
            {
                currentAction = ClientAction.None;
            }

            _currentActions[player] = @event.ClientActionState switch
            {
                ClientActionState.Enable => currentAction.EnableAction(@event.ClientAction.Value),
                ClientActionState.Disable => currentAction.DisableAction(@event.ClientAction.Value),
                _ => throw new ArgumentOutOfRangeException()
            };
        });
    }

    public void Terminate()
    {
        _eventQueue.Unsubscribe(EventType.ClientAction, _clientActionSubscription);
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

            if (!_currentActions.TryGetValue(player, out ClientAction currentAction))
            {
                continue;
            }

            BulletSpawnerComponent bulletSpawnerComponent = ownedSpaceship.GetComponent<BulletSpawnerComponent>()
                                                            ?? throw new ArgumentException();

            if (currentAction.HasFlag(ClientAction.TurnLeft))
            {
                ownedSpaceship.TurnLeft();
            }

            if (currentAction.HasFlag(ClientAction.TurnRight))
            {
                ownedSpaceship.TurnRight();
            }

            if (currentAction.HasFlag(ClientAction.Stop))
            {
                ownedSpaceship.Stop();
            }

            if (currentAction.HasFlag(ClientAction.Accelerate))
            {
                ownedSpaceship.Accelerate();
            }

            if (currentAction.HasFlag(ClientAction.Decelerate))
            {
                ownedSpaceship.Decelerate();
            }

            if (currentAction.HasFlag(ClientAction.Fire))
            {
                bulletSpawnerComponent.Fire = true;

                _currentActions[player] = currentAction.DisableAction(ClientAction.Fire);
            }
        }
    }
}