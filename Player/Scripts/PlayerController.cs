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
                }
            }
        }
    }
}