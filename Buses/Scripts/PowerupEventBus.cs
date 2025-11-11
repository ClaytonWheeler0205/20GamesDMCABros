using Godot;
using Util.ExtensionMethods;

namespace Game.Buses
{

    public class PowerupEventBus : Node
    {
        [Signal]
        public delegate void MushroomCollected();
        [Signal]
        public delegate void FlowerCollected();

        private static PowerupEventBus _instance;
        public static PowerupEventBus Instance
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