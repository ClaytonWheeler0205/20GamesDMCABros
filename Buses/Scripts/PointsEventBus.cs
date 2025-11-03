using Godot;
using Util.ExtensionMethods;

namespace Game.Buses
{

    public class PointsEventBus : Node
    {
        [Signal]
        public delegate void PointsGained(int pointValue);

        private static PointsEventBus _instance;
        public static PointsEventBus Instance
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