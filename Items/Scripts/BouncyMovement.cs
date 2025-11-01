using Godot;
using Util.ExtensionMethods;

namespace Game.Items
{

    public class BouncyMovement : Node
    {
        [Export]
        private float _speed = 45.0f;
        [Export]
        private KinematicBody2D _bodyToMove;
        public KinematicBody2D BodyToMove
        {
            get { return _bodyToMove; }
            set
            {
                if (value.IsValid())
                {
                    _bodyToMove = value;
                }
            }
        }
        private Vector2 _velocity;
        private bool _canMove = false;
        public bool CanMove
        {
            set
            {
                _canMove = value;
            }
        }

        public override void _Ready()
        {
            _velocity = _speed * Vector2.Right;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_canMove)
            {
                _velocity.y += GlobalWorldData.GRAVITY * delta;
                KinematicCollision2D collision = _bodyToMove.MoveAndCollide(delta * _velocity);
                if (collision.IsValid())
                {
                    HandleCollision(collision);
                }
            }
        }

        private void HandleCollision(KinematicCollision2D collision)
        {
            _velocity = _velocity.Bounce(collision.Normal);
        }

    }
}