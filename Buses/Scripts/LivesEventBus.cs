using Godot;
using Util.ExtensionMethods;

namespace Game.Buses
{

    public class LivesEventBus : Node
    {
        [Signal]
        public delegate void LoseLife();
        [Signal]
        public delegate void GainLife();
        [Signal]
        public delegate void GameOver();

        private static LivesEventBus _instance;
        public static LivesEventBus Instance
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