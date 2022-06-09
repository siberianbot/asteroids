using System.Diagnostics;
using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Scenes;

namespace Asteroids.Server;

public class Server
{
    private readonly CommandQueue _commandQueue = new CommandQueue();
    private readonly EventQueue _eventQueue = new EventQueue();
    private readonly ControllersCollection _controllers = new ControllersCollection();
    private readonly Vars _vars = new Vars();
    private readonly Thread _serverThread;
    private bool _alive;

    public Server()
    {
        _serverThread = new Thread(ServerFunc);
    }

    public void Start()
    {
        if (_alive)
        {
            return;
        }

        _serverThread.Start();
    }

    public void Stop()
    {
        _alive = false;

        while (_serverThread.IsAlive)
        {
            // wait for finish
        }
    }

    public ControllersCollection Controllers
    {
        get => _controllers;
    }

    private void ServerFunc()
    {
        _commandQueue.Reset();
        _eventQueue.Reset();
        _controllers.Clear();

        BehaviorController behaviorController = new BehaviorController(_commandQueue, _eventQueue);
        _controllers.AddController(behaviorController);
        EntityController entityController = new EntityController(_commandQueue, _eventQueue);
        _controllers.AddController(entityController);
        _controllers.AddController(new PlayerController(_commandQueue, _eventQueue));

        Spawner spawner = new Spawner(
            _controllers.GetController<EntityController>(),
            _controllers.GetController<PlayerController>());

        BehaviorFactory behaviorFactory = new BehaviorFactory(
            _controllers,
            _commandQueue,
            _eventQueue,
            spawner,
            _vars);

        SceneManager sceneManager = new SceneManager(
            spawner,
            behaviorFactory,
            _controllers);

        SceneController sceneController = new SceneController(sceneManager, _commandQueue, _eventQueue, _vars);
        _controllers.AddController(sceneController);

        _controllers.InitializeAll();

        sceneController.ChangeScene(Constants.Scenes.AsteroidCollision);

        _alive = true;
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        
        while (_alive)
        {
            float delta = (float)stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();

            _eventQueue.ExecutePending();
            _commandQueue.ExecutePending();

            foreach (Entity entity in entityController.Entities)
            {
                entity.Update(delta);
            }

            foreach (IBehavior behavior in behaviorController.Behaviors)
            {
                behavior.Update(delta);
            }
        }

        _controllers.TerminateAll();
    }
}