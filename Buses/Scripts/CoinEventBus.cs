using Godot;
using Util.ExtensionMethods;

namespace Game.Buses
{

    public class CoinEventBus : Node
    {
        [Signal]
        public delegate void CoinCollected();

        private static CoinEventBus _instance;
        public static CoinEventBus Instance
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