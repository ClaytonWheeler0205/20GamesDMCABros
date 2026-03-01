using Godot;
using Util.ExtensionMethods;

namespace Game
{
    public enum JingleType
    {
        Starman,
        CourseClear,
        CastleClear,
        Ending,
        Death,
        GameOver,
        Hurry
    }

    public abstract class JinglePlayer : AudioStreamPlayer
    {

        private static JinglePlayer _instance;
        public static JinglePlayer Instance
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

        public abstract void PlayJingle(JingleType jingleToPlay);
        public abstract void StopJingle();
        public abstract void OnJingleFinished();
    }
}