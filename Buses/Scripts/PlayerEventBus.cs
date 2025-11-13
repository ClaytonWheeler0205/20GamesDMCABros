using Godot;
using Util.ExtensionMethods;

namespace Game.Buses
{

    public class PlayerEventBus : Node
    {
        [Signal]
        public delegate void FireballThrown();

        private static PlayerEventBus _instance;
        public static PlayerEventBus Instance
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