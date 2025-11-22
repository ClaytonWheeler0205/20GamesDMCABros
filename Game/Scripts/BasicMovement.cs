using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    public abstract class BasicMovement : Node2D
    {
        [Export]
        private float _speed = 90.0f;
        public float Speed
        {
            set
            {
                if (value <= 0.0f)
                {
                    return;
                }
                _speed = value;
            }
        }
        [Export]
        private NodePath _wallDetectorPath;
        private RayCast2D _wallDetectorReference;
        protected RayCast2D WallDetectorReference
        {
            get { return _wallDetectorReference; }
        }
        [Export]
        private Direction _movementDirection = Direction.Right;
        public Direction MovementDirection
        {
            get { return _movementDirection; }
            protected set { _movementDirection = value; }
        }
        private KinematicBody2D _bodyToMove;
        public KinematicBody2D BodyToMove
        {
            protected get { return _bodyToMove; }
            set
            {
                if (value.IsValid())
                {
                    _bodyToMove = value;
                }
            }
        }
        private bool _shouldMove = false;
        public bool ShouldMove
        {
            set { _shouldMove = value; }
        }
        private Vector2 _velocity = new Vector2();
        public Vector2 Velocity
        {
            protected get { return _velocity; }
            set { _velocity = value; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            _wallDetectorReference.CastTo = (int)_movementDirection * _wallDetectorReference.CastTo;
        }

        private void SetNodeReferences()
        {
            _wallDetectorReference = GetNode<RayCast2D>(_wallDetectorPath);
        }

        public override void _Process(float delta)
        {
            if (_shouldMove)
            {
                _velocity.x = (int)_movementDirection * _speed;
                _velocity.y += GlobalWorldData.GRAVITY * delta;
                if (_velocity.y > GlobalWorldData.TERMINAL_VELOCITY)
                {
                    _velocity.y = GlobalWorldData.TERMINAL_VELOCITY;
                }
                _velocity = _bodyToMove.MoveAndSlide(_velocity, Vector2.Up);
                CheckForDirectionChange();
            }
        }

        private void CheckForDirectionChange()
        {
            if (_wallDetectorReference.IsColliding())
            {
                FlipDirection();
            }
        }

        public abstract void FlipDirection();
    }
}