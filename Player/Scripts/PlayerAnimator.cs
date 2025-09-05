using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public abstract class PlayerAnimator : AnimatedSprite
    {
        private Vito _playerToAnimate;
        public Vito PlayerToAnimate
        {
            get { return _playerToAnimate; }
            set
            {
                if (value.IsValid())
                {
                    _playerToAnimate = value;
                }
            }
        }
        private MovementComponent _playerMovement;
        public MovementComponent PlayerMovement
        {
            get { return _playerMovement; }
            set
            {
                if (value.IsValid())
                {
                    _playerMovement = value;
                }
            }
        }
        private JumpComponent _playerJump;
        public JumpComponent PlayerJump
        {
            get { return _playerJump; }
            set
            {
                if (value.IsValid())
                {
                    _playerJump = value;
                }
            }
        }
    }
}