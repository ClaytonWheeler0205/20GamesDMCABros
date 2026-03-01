using Godot;
using Util.ExtensionMethods;

public class LevelEventBus : Node
{
    [Signal]
    public delegate void HurryJingleFinished();

    private static LevelEventBus _instance;
    public static LevelEventBus Instance
    {
        get { return _instance; }
    }

    public override void _Ready()
    {
        if (_instance != null && _instance != this)
        {
            this.SafeQueueFree();
            return;
        }
        _instance = this;
    }

}
