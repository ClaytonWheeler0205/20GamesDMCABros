using Game.Buses;
using Godot;
using Util.ExtensionMethods;

namespace Game
{
    public enum JingleType
    {
        Starman,
        StarmanFast,
        CourseClear,
        CastleClear,
        Ending,
        Death,
        GameOver,
        Hurry,
        None
    }

    public abstract class JinglePlayer : AudioStreamPlayer
    {

        private static JinglePlayer _instance;
        public static JinglePlayer Instance
        {
            get { return _instance; }
        }
        private bool _hurryJinglePlayed;
        protected bool HurryJinglePlayed
        {
            get { return _hurryJinglePlayed; }
            set { _hurryJinglePlayed = value; }
        }

        public override void _Ready()
        {
            if (_instance != null && _instance != this)
            {
                this.SafeQueueFree();
                return;
            }
            _instance = this;
            PowerupEventBus.Instance.Connect("StarEnding", this, nameof(OnStarEnding));
        }

        public abstract void PlayJingle(JingleType jingleToPlay);
        public abstract void StopJingle();
        public abstract void OnJingleFinished();
        public abstract void OnStarEnding();
    }
}
