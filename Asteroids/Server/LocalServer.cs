using System.Diagnostics;
using Asteroids.Behaviors;
using Asteroids.Controllers;
using Asteroids.Engine;
using Asteroids.Entities;
using Asteroids.Scenes;

namespace Asteroids.Server;

public class LocalServer : IServer
{
    private readonly EventQueue _eventQueue = new EventQueue();
    private readonly ControllersCollection _controllers = new ControllersCollection();
    private readonly Vars _vars;
    private readonly Thread _serverThread;
    private readonly List<IClient> _clients = new List<IClient>();
    private bool _alive;

    public LocalServer(Vars vars)
    {
        _vars = vars;
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

    public void Push(Event @event)
    {
        _eventQueue.Push(@event);
    }

    public ServerState State { get; private set; } = ServerState.Stopped;

    public IEntityCollection? EntityCollection { get; private set; }

    public IEnumerable<IClient> Clients
    {
        get => _clients;
    }

    private void ServerFunc()
    {
        State = ServerState.Initializing;

        _eventQueue.Reset();
        _controllers.Clear();

        BehaviorController behaviorController = new BehaviorController(_eventQueue);
        _controllers.AddController(behaviorController);

        EntityController entityController = new EntityController(_eventQueue);
        _controllers.AddController(entityController);
        EntityCollection = entityController;

        _controllers.AddController(new PlayerController(_eventQueue));

        Spawner spawner = new Spawner(entityController);

        BehaviorFactory behaviorFactory = new BehaviorFactory(
            _controllers,
            _eventQueue,
            spawner,
            _vars);

        SceneManager sceneManager = new SceneManager(
            spawner,
            behaviorFactory,
            _controllers);

        _controllers.AddController(new SceneController(sceneManager, _eventQueue, _vars));

        _controllers.InitializeAll();

        long clientConnectedSubscriptionIdx = _eventQueue.Subscribe(EventType.ClientConnected, @event =>
        {
            @event.Client!.Player = spawner.SpawnPlayer(Constants.Colors.Green);
            @event.Client!.Camera = spawner.SpawnCamera(@event.Client!.Player);

            _clients.Add(@event.Client);
        });

        long clientDisconnectedSubscriptionIdx = _eventQueue.Subscribe(EventType.ClientDisconnected, @event =>
        {
            entityController.DestroyEntity(@event.Client!.Player!);
            entityController.DestroyEntity(@event.Client!.Camera!);

            @event.Client!.Player = null;
            @event.Client!.Camera = null;

            _clients.Remove(@event.Client);
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

        // TODO: handle client disconnection
        _clients.Clear();

        _eventQueue.Unsubscribe(EventType.ClientConnected, clientConnectedSubscriptionIdx);
        _eventQueue.Unsubscribe(EventType.ClientDisconnected, clientDisconnectedSubscriptionIdx);
        _controllers.TerminateAll();

        State = ServerState.Stopped;
    }
}