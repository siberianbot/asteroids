using Asteroids.Engine;

namespace Asteroids.UI;

public abstract class ClientUI : IClientUI
{
    private readonly Vars _vars;
    private readonly string _uiVarName;

    protected ClientUI(Vars vars, string uiVarName)
    {
        _vars = vars;
        _uiVarName = uiVarName;
    }

    public void Update()
    {
        if (!_vars.GetVar(_uiVarName, false))
        {
            return;
        }

        OnUpdate();
    }

    protected void SetVisibility(bool visible)
    {
        _vars.SetVar(_uiVarName, visible);
    }

    protected abstract void OnUpdate();
}