using Godot;
using Util.ExtensionMethods;

public class LevelEventBus : Node
{
    [Signal]
    public delegate void HurryJingleFinished();
    [Signal]
    public delegate void PipeEntranceFinished(bool playExitAnimation);
    [Signal]
    public delegate void PipeTransitionFinished(bool playExitAnimation);
    [Signal]
    public delegate void LevelFinished();
    [Signal]
    public delegate void LevelWalkStarted();
    [Signal]
    public delegate void LevelWalkFinished();

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
