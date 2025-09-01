using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public abstract class JumpComponent : Node
    {
        [Signal]
        public delegate void SuccessfulJump();
        [Signal]
        public delegate void JumpReleased();
        private KinematicBody2D _jumpingBody;
        public KinematicBody2D JumpingBody
        {
            get { return _jumpingBody; }
            set
            {
                if (value.IsValid())
                {
                    _jumpingBody = value;
                }
            }
        }
        [Export]
        private float _baseGravity = 1200.0f;
        protected float BaseGravity
        {
            get { return _baseGravity; }
        }
        [Export]
        private float _jumpPower = -400.0f;
        public float JumpPower
        {
            get { return _jumpPower; }
        }

        public abstract float GetGravity(float yDirection);
        public abstract void AttemptJump();
        public abstract void ReleaseJump();
    }
}