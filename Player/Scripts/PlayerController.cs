using Game.Buses;
using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{
    public abstract class PlayerController : Node
    {
        private bool _isControllerActive = true;
        public bool IsControllerActive
        {
            get { return _isControllerActive; }
            set { _isControllerActive = value; }
        }
        private Vito _characterToControl;
        public Vito CharacterToControl
        {
            get { return _characterToControl; }
            set
            {
                if (value.IsValid())
                {
                    _characterToControl = value;
                    _characterToControl.Connect("PlayerFrozen", this, nameof(OnPlayerFrozen));
                }
            }
        }

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("PipeEntered", this, nameof(OnPipeEntered));
            LevelEventBus.Instance.Connect("PipeTransitionFinished", this, nameof(OnPipeTransitionFinished));
            PlayerEventBus.Instance.Connect("PipeExitAnimationFinished", this, nameof(OnPipeExitAnimationFinished));
        }

        public abstract void OnPipeEntered();
        public abstract void OnPipeTransitionFinished(bool playExitAnimation);
        public abstract void OnPipeExitAnimationFinished();
        public abstract void OnPlayerFrozen();
    }
}
