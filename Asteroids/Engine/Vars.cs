namespace Asteroids.Engine;

// TODO: do not use object as value
public class Vars
{
    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    public void SetVar(string name, object value)
    {
        _data[name] = value;
    }

    public TValue GetVar<TValue>(string name, TValue defaultValue)
    {
        if (_data.ContainsKey(name))
        {
            return (TValue)_data[name];
        }

        return defaultValue;
    }
}