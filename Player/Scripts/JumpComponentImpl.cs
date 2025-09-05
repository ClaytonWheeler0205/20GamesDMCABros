using Godot;

namespace Game.Player
{

    public class JumpComponentImpl : JumpComponent
    {
        [Export]
        private float _fastFallingScale = 1.5f;
        private bool _jumpReleased = false;
        [Export]
        private NodePath _jumpBufferPath;
        private Timer _jumpBuffer;
        [Export]
        private float _jumpBufferTime = 0.05f;
        private bool _jumpBufferStored = false;
        [Export]
        private NodePath _coyoteTimerPath;
        private Timer _coyoteTimer;
        [Export]
        private float _coyoteTime = 0.05f;
        private bool _coyoteTimerActivated = false;
        private bool _coyoteTimerTimedout = false;
        private bool _performedJump = false;
        [Export]
        private NodePath _jumpSoundPath;
        private AudioStreamPlayer _jumpSoundReference;

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _jumpBuffer = GetNode<Timer>(_jumpBufferPath);
            _coyoteTimer = GetNode<Timer>(_coyoteTimerPath);
            _jumpSoundReference = GetNode<AudioStreamPlayer>(_jumpSoundPath);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (JumpingBody.IsOnFloor())
            {
                _performedJump = false;
                _coyoteTimerTimedout = false;
            }
            if (JumpingBody.IsOnFloor() && _jumpBufferStored && !_performedJump)
            {
                _jumpBufferStored = false;
                PerformJump();
            }
            else if (!JumpingBody.IsOnFloor() && !_coyoteTimerActivated && !_performedJump && !_coyoteTimerTimedout)
            {
                _coyoteTimerActivated = true;
                _coyoteTimer.Start(_coyoteTime);
            }
        }

        public override float GetGravity(float yDirection)
        {
            if (yDirection < 0)
            {
                return BaseGravity;
            }
            else
            {
                return BaseGravity * _fastFallingScale;
            }
        }

        public override void AttemptJump()
        {
            if (JumpingBody.IsOnFloor() && !_performedJump)
            {
                PerformJump();
            }
            else if (!JumpingBody.IsOnFloor() && _coyoteTimerActivated && !_performedJump)
            {
                _coyoteTimerActivated = false;
                PerformJump();
            }
            else if (!JumpingBody.IsOnFloor() && !_jumpBufferStored)
            {
                _jumpBufferStored = true;
                _jumpBuffer.Start(_jumpBufferTime);
            }
        }

        private void PerformJump()
        {
            _performedJump = true;
            _jumpReleased = false;
            _jumpSoundReference.Play();
            EmitSignal("SuccessfulJump");
        }

        public override void ReleaseJump()
        {
            if (!JumpingBody.IsOnFloor() && !_jumpReleased)
            {
                _jumpReleased = true;
                EmitSignal("JumpReleased");
            }
        }

        public void OnBufferTimerTimeout()
        {
            _jumpBufferStored = false;
        }

        public void OnCoyoteTimerTimeout()
        {
            _coyoteTimerTimedout = true;
            _coyoteTimerActivated = false;
        }
    }
}