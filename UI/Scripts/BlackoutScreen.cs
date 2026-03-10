using Godot;

namespace Game.UI
{

    public class BlackoutScreen : Control
    {

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            LevelEventBus.Instance.Connect("PipeEntranceFinished", this, nameof(OnPipeEntranceFinished));
        }

        private void OnPipeEntranceFinished()
        {
            Visible = true;
        }
    }
}