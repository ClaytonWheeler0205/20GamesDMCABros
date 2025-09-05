using Godot;

namespace Game.Player
{

    public class VitoImpl : Vito
    {
        private Vector2 _velocity = new Vector2();

        public override void _Process(float delta)
        {
            MovementComponentReference.Direction = Input.GetAxis("move_left", "move_right");
        }

        public override void _PhysicsProcess(float delta)
        {
            _velocity.y += JumpComponentReference.GetGravity(_velocity.y) * delta;
            _velocity.x = MovementComponentReference.GetMovementSpeed(_velocity.x);
            _velocity = MoveAndSlide(_velocity, Vector2.Up);
        }

        public override void Jump()
        {
            JumpComponentReference.AttemptJump();
        }

        public override void ReleaseJump()
        {
            JumpComponentReference.ReleaseJump();
        }

        public override void StartRunning()
        {
            MovementComponentReference.StartRunning();
        }

        public override void StopRunning()
        {
            MovementComponentReference.StopRunning();
        }

        public override void OnSuccessfulJump()
        {
            if (_velocity.y < 0.0f)
            {
                GD.PushError("UH OH");
            }
            if (Mathf.Abs(_velocity.x) >= JumpComponentReference.SuperJumpSpeedRequirement)
            {
                _velocity.y = JumpComponentReference.SuperJumpPower;
            }
            else
            {
                _velocity.y = JumpComponentReference.JumpPower;
            }
        }

        public override void OnJumpReleased()
        {
            if (_velocity.y < 0.0f)
            {
                _velocity.y = 0.5f * _velocity.y;
            }
        }

        public override Vector2 GetVelocityVector()
        {
            return _velocity;
        }


        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("jump"))
            {
                Jump();
            }
            else if (@event.IsActionReleased("jump"))
            {
                ReleaseJump();
            }
            if (@event.IsActionPressed("run"))
            {
                StartRunning();
            }
            else if (@event.IsActionReleased("run"))
            {
                StopRunning();
            }
        }
    }
}