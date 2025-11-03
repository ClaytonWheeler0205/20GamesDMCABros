using Godot;
using Util.ExtensionMethods;

namespace Game.Buses
{
    public class TimerEventBus : Node
    {
        [Signal]
        public delegate void TimeUp();
        [Signal]
        public delegate void TimeLow();

        private static TimerEventBus _instance;
        public static TimerEventBus Instance
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
}