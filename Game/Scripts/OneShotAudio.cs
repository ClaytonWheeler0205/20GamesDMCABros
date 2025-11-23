using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public class OneShotAudio : AudioStreamPlayer
    {
        public override void _Ready()
        {
            SetNodeConnections();
            Play();
        }

        private void SetNodeConnections()
        {
            Connect("finished", this, nameof(OnSoundFinished));
        }

        public void OnSoundFinished()
        {
            this.SafeQueueFree();
        }
    }
}