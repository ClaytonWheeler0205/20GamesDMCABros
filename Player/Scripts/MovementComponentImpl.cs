using Godot;

namespace Game.Player
{

    public class MovementComponentImpl : MovementComponent
    {
        [Export]
        private float _friction = 0.1f;
        [Export]
        private float _acceleration = 0.25f;

        public override float GetMovementSpeed(float currentSpeed)
        {
            if (Direction != 0.0f)
            {
                return GetAcceleratingSpeed(currentSpeed);
            }
            else
            {
                return Mathf.Lerp(currentSpeed, 0.0f, _friction);
            }
        }

        private float GetAcceleratingSpeed(float currentSpeed)
        {
            if (IsRunning)
            {
                return Mathf.Lerp(currentSpeed, Direction * RunSpeed, _acceleration);
            }
            else
            {
                return Mathf.Lerp(currentSpeed, Direction * WalkSpeed, _acceleration);
            }
        }

        public override void StartRunning()
        {
            IsRunning = true;
        }

        public override void StopRunning()
        {
            IsRunning = false;
        }
    }
}