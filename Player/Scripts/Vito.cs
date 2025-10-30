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
        private NodePath _playerVisualPath;
        private PlayerAnimator _playerVisualReference;
        protected PlayerAnimator PlayerVisualReference
        {
            get { return _playerVisualReference; }
        }
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
            _playerVisualReference.PlayerToAnimate = this;
            _playerVisualReference.PlayerJump = _jumpComponentReference;
            _playerVisualReference.PlayerMovement = _movementComponentReference;
        }

        private void SetNodeReferences()
        {
            _jumpComponentReference = GetNode<JumpComponent>(_jumpComponentPath);
            _movementComponentReference = GetNode<MovementComponent>(_movementComponentPath);
            _playerVisualReference = GetNode<PlayerAnimator>(_playerVisualPath);
            _jumpHitDataReference = GetNode<JumpHitbox>(_jumpHitDataPath);
        }

        private void SetNodeConnections()
        {
            _jumpComponentReference.Connect("SuccessfulJump", this, nameof(OnSuccessfulJump));
            _jumpComponentReference.Connect("JumpReleased", this, nameof(OnJumpReleased));
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