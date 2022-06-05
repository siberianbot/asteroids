using System.Numerics;
using Asteroids.Components;
using Asteroids.Controllers;
using Asteroids.Entities;

namespace Asteroids.Behaviors;

public class PlayerSpawningBehavior : IBehavior
{
    private const float Cooldown = 5.0f;

    private readonly CameraController _cameraController;
    private readonly PlayerController _playerController;
    private readonly Spawner _spawner;
    private readonly Dictionary<Player, float> _cooldown = new Dictionary<Player, float>();

    public PlayerSpawningBehavior(CameraController cameraController, PlayerController playerController, Spawner spawner)
    {
        _cameraController = cameraController;
        _playerController = playerController;
        _spawner = spawner;
    }

    public void Update(float delta)
    {
        foreach (Player player in _playerController.Players)
        {
            if (player.Alive)
            {
                continue;
            }

            if (!_cooldown.ContainsKey(player))
            {
                _cooldown[player] = Cooldown;
            }

            _cooldown[player] += delta;

            if (_cooldown[player] <= Cooldown)
            {
                continue;
            }

            // TODO: randomize position
            Spaceship spaceship = _spawner.SpawnSpaceship(Vector2.Zero, player, player.Color);
            spaceship.AddComponent(new SpaceshipControlComponent());
            _cameraController.CurrentCamera = _spawner.SpawnCamera(spaceship);

            _cooldown[player] = 0;
        }
    }
}