using Godot;

namespace Game.Player
{

    public class VitoImpl : Vito
    {
        private Vector2 _velocity = new Vector2();

        public override void _PhysicsProcess(float delta)
        {
            _velocity.y += JumpComponentReference.GetGravity(_velocity.y) * delta;
            if (_velocity.y > JumpComponentReference.TerminalVelocity)
            {
                _velocity.y = JumpComponentReference.TerminalVelocity;
            }
            _velocity.x = MovementComponentReference.GetMovementSpeed(_velocity.x);
            AttemptCornerCorrection(3);
            _velocity = MoveAndSlide(_velocity, Vector2.Up);
            JumpHitDataReference.VerticalVelocity = _velocity.y;
            if (IsOnFloor())
            {
                JumpHitDataReference.HasHitBlock = false;
            }
        }

        private void AttemptCornerCorrection(int amount)
        {
            float delta = GetPhysicsProcessDeltaTime();
            if (_velocity.y < 0 && TestMove(GlobalTransform, new Vector2(0, _velocity.y * delta)))
            {
                for (int i = 1; i < amount + 1; i++)
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        if (!TestMove(GlobalTransform.Translated(new Vector2(i * j, 0)), new Vector2(0, _velocity.y * delta)))
                        {
                            Translate(new Vector2(i * j, 0));
                            return;
                        }
                    }
                }
            }
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

        public override void SetMovementDirection(float newDirection)
        {
            MovementComponentReference.Direction = newDirection;
        }
    }
}