using Game.Items;
using Godot;
using Util.ExtensionMethods;

namespace Game.Projectiles
{
    public enum BallType
    {
        fire,
        ice
    }

    public abstract class Fireball : KinematicBody2D
    {
        [Signal]
        public delegate void FireballDestroyed();

        [Export]
        private NodePath _movementPath;
        private BouncyMovement _movementReference;
        protected BouncyMovement MovementReference
        {
            get { return _movementReference; }
        }
        [Export]
        private NodePath _visualPath;
        private AnimatedSprite _visualReference;
        protected AnimatedSprite VisualReference
        {
            get { return _visualReference; }
        }
        [Export]
        private NodePath _topWallDetectorPath;
        private RayCast2D _topWallDetectorReference;
        protected RayCast2D TopWallDetectorReference
        {
            get { return _topWallDetectorReference; }
        }
        [Export]
        private NodePath _bottomWallDetectorPath;
        private RayCast2D _bottomWallDetectorReference;
        protected RayCast2D BottomWallDetectorReference
        {
            get { return _bottomWallDetectorReference; }
        }
        [Export]
        private NodePath _hitboxPath;
        private CollisionShape2D _hitboxReference;
        protected CollisionShape2D HitboxReference
        {
            get { return _hitboxReference; }
        }
        [Export]
        private NodePath _fireballSoundPath;
        private AudioStreamPlayer _fireballSoundReference;
        protected AudioStreamPlayer FireballSoundReference
        {
            get { return _fireballSoundReference; }
        }
        private BallType _ballElement = BallType.fire;
        public BallType BallElement
        {
            set
            {
                _ballElement = value;
                SetupBallElement();
            }
        }
        private Direction _movementDirection = Direction.Right;
        public Direction MovementDirection
        {
            set
            {
                _movementDirection = value;
                ApplyDirection();
            }
        }
        private bool _enabled = false;
        public bool Enabled
        {
            get { return _enabled; }
            protected set { _enabled = value; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            _movementReference.BodyToMove = this;
            SetupBallElement();
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<BouncyMovement>(_movementPath);
            _visualReference = GetNode<AnimatedSprite>(_visualPath);
            _topWallDetectorReference = GetNode<RayCast2D>(_topWallDetectorPath);
            _bottomWallDetectorReference = GetNode<RayCast2D>(_bottomWallDetectorPath);
            _hitboxReference = GetNode<CollisionShape2D>(_hitboxPath);
            _fireballSoundReference = GetNode<AudioStreamPlayer>(_fireballSoundPath);
        }

        private void SetupBallElement()
        {
            if (_ballElement == BallType.fire)
            {
                AddToGroup("fire");
                if (!IsInGroup("ice"))
                {
                    return;
                }
                RemoveFromGroup("ice");
            }
            else
            {
                AddToGroup("ice");
                if (!IsInGroup("fire"))
                {
                    return;
                }
                RemoveFromGroup("fire");
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (!_topWallDetectorReference.IsColliding() && !_bottomWallDetectorReference.IsColliding())
            {
                return;
            }
            DestroyFireball();
        }

        private void ApplyDirection()
        {
            _topWallDetectorReference.CastTo = (int)_movementDirection * new Vector2(5, 0);
            _bottomWallDetectorReference.CastTo = (int)_movementDirection * new Vector2(5, 0);
            _movementReference.MovementDirection = _movementDirection;
            _visualReference.FlipH = _movementDirection == Direction.Left;
        }

        protected void DestroyFireball()
        {
            _movementReference.CanMove = false;
            _visualReference.Play("explosion");
        }

        public abstract void Enable();
    }
}