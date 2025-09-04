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

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            _jumpComponentReference.JumpingBody = this;
            _movementComponentReference.MovingBody = this;
        }

        private void SetNodeReferences()
        {
            _jumpComponentReference = GetNode<JumpComponent>(_jumpComponentPath);
            _movementComponentReference = GetNode<MovementComponent>(_movementComponentPath);
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
    }
}