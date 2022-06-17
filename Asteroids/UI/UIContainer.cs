namespace Asteroids.UI;

public class UIContainer
{
    private const int MaxUICount = 4;
    private readonly IClientUI?[] _uis = new IClientUI[MaxUICount];

    public void Set(int idx, IClientUI clientUI)
    {
        _uis[idx] = clientUI ?? throw new ArgumentNullException(nameof(clientUI));
        _uis[idx]!.Initialize();
    }

    public void Unset(int idx)
    {
        _uis[idx] = null;
    }

    public void Update()
    {
        foreach (IClientUI? clientUI in _uis)
        {
            clientUI?.Update();
        }
    }

    public void Clear()
    {
        for (int idx = 0; idx < MaxUICount; idx++)
        {
            Unset(idx);
        }
    }
}