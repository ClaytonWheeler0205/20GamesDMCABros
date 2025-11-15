using Game.Projectiles;
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
        protected PlayerAnimator SmallPlayerVisualReference
        {
            get { return _smallPlayerVisualReference; }
        }
        [Export]
        private NodePath _superPlayerVisualPath;
        private PlayerAnimator _superPlayerVisualReference;
        protected PlayerAnimator SuperPlayerVisualReference
        {
            get { return _superPlayerVisualReference; }
        }
        [Export]
        private NodePath _jumpHitDataPath;
        private JumpHitbox _jumpHitDataReference;
        protected JumpHitbox JumpHitDataReference
        {
            get { return _jumpHitDataReference; }
        }
        [Export]
        private NodePath _fireballPoolPath;
        private FireballFactory _fireballPoolReference;
        protected FireballFactory FireballPoolReference
        {
            get { return _fireballPoolReference; }
        }
        private Vector2 _rightFireballSpawn = new Vector2(7, -10);
        protected Vector2 RightFireballSpawn
        {
            get { return _rightFireballSpawn; }
        }
        private Vector2 _leftFireballSpawn = new Vector2(-7, -10);
        protected Vector2 LeftFireballSpawn
        {
            get { return _leftFireballSpawn; }
        }
        private bool _hasFlower = false;
        protected bool HasFlower
        {
            get { return _hasFlower; }
            set { _hasFlower = value; }
        }
        private bool _canThrow = true;
        protected bool CanThrow
        {
            get { return _canThrow; }
            set { _canThrow = value; }
        }
        [Export]
        private NodePath _paletteComponentPath;
        private VitoPaletteComponent _paletteComponentReference;
        [Export]
        private NodePath _paletteAnimatorPath;
        private AnimationPlayer _paletteAnimatorReference;
        protected AnimationPlayer PaletteAnimatorReference
        {
            get { return _paletteAnimatorReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            _jumpComponentReference.JumpingBody = this;
            _movementComponentReference.MovingBody = this;
            SetupPlayerVisuals();
            _paletteComponentReference.PlayerMaterial = (ShaderMaterial)Material;
        }

        private void SetNodeReferences()
        {
            _jumpComponentReference = GetNode<JumpComponent>(_jumpComponentPath);
            _movementComponentReference = GetNode<MovementComponent>(_movementComponentPath);
            _smallPlayerVisualReference = GetNode<PlayerAnimator>(_smallPlayerVisualPath);
            _superPlayerVisualReference = GetNode<PlayerAnimator>(_superPlayerVisualPath);
            _jumpHitDataReference = GetNode<JumpHitbox>(_jumpHitDataPath);
            _fireballPoolReference = GetNode<FireballFactory>(_fireballPoolPath);
            _paletteComponentReference = GetNode<VitoPaletteComponent>(_paletteComponentPath);
            _paletteAnimatorReference = GetNode<AnimationPlayer>(_paletteAnimatorPath);
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
        public abstract void StartCrouching();
        public abstract void StopCrouching();
        public abstract void ShootFireball();
        public abstract void OnSuccessfulJump();
        public abstract void OnJumpReleased();
        public abstract Vector2 GetVelocityVector();
        public abstract void SetMovementDirection(float newDirection);
    }
}