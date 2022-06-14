using System.Diagnostics;
using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Scenes;

namespace Asteroids.Server;

public class LocalServer : IServer
{
    private readonly CommandQueue _commandQueue = new CommandQueue();
    private readonly Vars _vars = new Vars();
    private readonly ControllersCollection _controllers = new ControllersCollection();
    private readonly EventQueue _eventQueue;
    private readonly Thread _serverThread;
    private bool _alive;

    public LocalServer(EventQueue eventQueue)
    {
        _eventQueue = eventQueue;
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

    public IClient Join(string name)
    {
        LocalClient client = new LocalClient
        {
            Name = name,
            Player = null
        };

        _eventQueue.Push(new Event
        {
            EventType = EventType.ClientConnected,
            Client = client
        });

        return client;
    }

    public void Leave(IClient client)
    {
        _eventQueue.Push(new Event
        {
            EventType = EventType.ClientDisconnected,
            Client = client
        });
    }

    public ServerState State { get; private set; } = ServerState.Stopped;

    public IEntityCollection? EntityCollection { get; private set; }

    private void ServerFunc()
    {
        State = ServerState.Initializing;

        _commandQueue.Reset();
        _eventQueue.Reset();
        _controllers.Clear();

        BehaviorController behaviorController = new BehaviorController(_commandQueue, _eventQueue);
        _controllers.AddController(behaviorController);

        EntityController entityController = new EntityController(_commandQueue, _eventQueue);
        _controllers.AddController(entityController);
        EntityCollection = entityController;

        PlayerController playerController = new PlayerController(_commandQueue, _eventQueue);
        _controllers.AddController(playerController);

        Spawner spawner = new Spawner(entityController, playerController);

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

        _controllers.AddController(new SceneController(sceneManager, _commandQueue, _eventQueue, _vars));

        _controllers.InitializeAll();

        long clientConnectedSubscriptionIdx = _eventQueue.Subscribe(EventType.ClientConnected, @event =>
        {
            @event.Client!.Player = spawner.SpawnPlayer(@event.Client!.Name, Constants.Colors.Green);

            playerController.AddPlayer(@event.Client!.Player);
        });

        long clientDisconnectedSubscriptionIdx = _eventQueue.Subscribe(EventType.ClientDisconnected, @event =>
        {
            @event.Client!.Player = null;

            playerController.RemovePlayer(@event.Client!.Player!);
        });

        _alive = true;
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        State = ServerState.Alive;

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

        State = ServerState.Stopping;

        _eventQueue.Unsubscribe(EventType.ClientConnected, clientConnectedSubscriptionIdx);
        _eventQueue.Unsubscribe(EventType.ClientDisconnected, clientDisconnectedSubscriptionIdx);
        _controllers.TerminateAll();

        State = ServerState.Stopped;
    }
}