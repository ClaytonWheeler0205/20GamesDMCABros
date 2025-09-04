using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{
    public abstract class MovementComponent : Node
    {
        [Export]
        private float _walkSpeed = 100.0f;
        protected float WalkSpeed
        {
            get { return _walkSpeed; }
        }
        [Export]
        private float _runSpeed = 150.0f;
        protected float RunSpeed
        {
            get { return _runSpeed; }
        }
        private bool _isRunning = false;
        protected bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }
        private float _direction = 0.0f;
        public float Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        private KinematicBody2D _movingBody;
        public KinematicBody2D MovingBody
        {
            get { return _movingBody; }
            set
            {
                if (value.IsValid())
                {
                    _movingBody = value;
                }
            }
        }

        public abstract float GetMovementSpeed(float currentSpeed);
        public abstract void StartRunning();
        public abstract void StopRunning();
    }
}