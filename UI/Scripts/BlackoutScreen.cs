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

        private async void OnPipeEntranceFinished()
        {
            Visible = true;
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            Visible = false;
            LevelEventBus.Instance.EmitSignal("PipeTransitionFinished");
        }
    }
}