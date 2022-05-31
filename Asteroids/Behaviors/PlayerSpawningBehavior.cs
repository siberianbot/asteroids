using System.Numerics;
using Asteroids.Components;
using Asteroids.Entities;
using Asteroids.Rendering;

namespace Asteroids.Behaviors;

public class PlayerSpawningBehavior : IBehavior
{
    private const float Cooldown = 5.0f;

    private readonly Dictionary<Player, float> _cooldown = new Dictionary<Player, float>();

    public void Update(UpdateContext context)
    {
        foreach (Player player in context.DependencyContainer.PlayerController.Players)
        {
            if (player.Alive)
            {
                continue;
            }

            if (!_cooldown.ContainsKey(player))
            {
                _cooldown[player] = Cooldown;
            }

            _cooldown[player] += context.Delta;

            if (_cooldown[player] <= Cooldown)
            {
                continue;
            }

            // TODO: randomize position
            Spaceship spaceship = context.DependencyContainer.Spawner.SpawnSpaceship(Vector2.Zero, player, player.Color);
            spaceship.AddComponent(new SpaceshipControlComponent());
            context.DependencyContainer.CameraController.CurrentCamera = context.DependencyContainer.Spawner.SpawnCamera(spaceship);

            _cooldown[player] = 0;
        }
    }
}