using Godot;

namespace Game.Player
{

    public abstract class Vito : KinematicBody2D
    {
        [Export]
        private NodePath _jumpComponentPath;
        private JumpComponent _jumpComponentReference;
        protected JumpComponent JumpComponentReference
        {
            get { return _jumpComponentReference; }
        }
        [Export]
        private NodePath _movementComponentPath;
        private MovementComponent _movementComponentReference;
        protected MovementComponent MovementComponentReference
        {
            get { return _movementComponentReference; }
        }
        [Export]
        private NodePath _smallPlayerVisualPath;
        private PlayerAnimator _smallPlayerVisualReference;
        [Export]
        private NodePath _superPlayerVisualPath;
        private PlayerAnimator _superPlayerVisualReference;
        [Export]
        private NodePath _jumpHitDataPath;
        private JumpHitbox _jumpHitDataReference;
        protected JumpHitbox JumpHitDataReference
        {
            get { return _jumpHitDataReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            _jumpComponentReference.JumpingBody = this;
            _movementComponentReference.MovingBody = this;
            SetupPlayerVisuals();
        }

        private void SetNodeReferences()
        {
            _jumpComponentReference = GetNode<JumpComponent>(_jumpComponentPath);
            _movementComponentReference = GetNode<MovementComponent>(_movementComponentPath);
            _smallPlayerVisualReference = GetNode<PlayerAnimator>(_smallPlayerVisualPath);
            _superPlayerVisualReference = GetNode<PlayerAnimator>(_superPlayerVisualPath);
            _jumpHitDataReference = GetNode<JumpHitbox>(_jumpHitDataPath);
        }

        private void SetNodeConnections()
        {
            _jumpComponentReference.Connect("SuccessfulJump", this, nameof(OnSuccessfulJump));
            _jumpComponentReference.Connect("JumpReleased", this, nameof(OnJumpReleased));
        }

        private void SetupPlayerVisuals()
        {
            _smallPlayerVisualReference.PlayerToAnimate = this;
            _smallPlayerVisualReference.PlayerMovement = _movementComponentReference;
            _superPlayerVisualReference.PlayerToAnimate = this;
            _superPlayerVisualReference.PlayerMovement = _movementComponentReference;
        }

        public abstract void Jump();
        public abstract void ReleaseJump();
        public abstract void StartRunning();
        public abstract void StopRunning();
        public abstract void OnSuccessfulJump();
        public abstract void OnJumpReleased();
        public abstract Vector2 GetVelocityVector();
        public abstract void SetMovementDirection(float newDirection);
    }
}