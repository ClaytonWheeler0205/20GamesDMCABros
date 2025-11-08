using System;
using Game.Util;
using Godot;

namespace Game.Player
{

    public class MovementComponentImpl : MovementComponent
    {
        [Export]
        private float _walkAcceleration = 0.25f;
        [Export]
        private float _runAcceleration = 0.3f;
        [Export]
        private float _releaseDeceleration = 0.1f;
        [Export]
        private float _skiddingDeceleration = 0.4f;
        [Export]
        private float _skidTurnaroundSpeed = 50.0f;
        [Export]
        private NodePath _runReleaseTimerPath;
        private Timer _runReleaseTimerReference;
        [Export]
        private float _runReleaseTime = 0.167f;
        private bool _wasRunning = false;
        [Export]
        private float _slowAirAcceleration = 0.25f;
        [Export]
        private float _fastAirAcceleration = 0.3f;
        [Export]
        private float _fastAirDeceleration = 0.3f;
        [Export]
        private float _mediumAirDeceleration = 0.275f;
        [Export]
        private float _slowAirDeceleration = 0.25f;
        [Export]
        private float _slowAirDecelerationSpeedLimit = 125.0f;
        private float _speedBeforeJump;

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _runReleaseTimerReference = GetNode<Timer>(_runReleaseTimerPath);
        }

        public override float GetMovementSpeed(float currentSpeed)
        {
            if (MovingBody.IsOnFloor())
            {
                _speedBeforeJump = GetGroundSpeed(currentSpeed);
                return _speedBeforeJump;
            }
            else
            {
                _wasRunning = false;
                return GetAirSpeed(currentSpeed);
            }
        }

        private float GetGroundSpeed(float currentSpeed)
        {
            if (Direction != 0.0f && !IsCrouched)
            {
                return GetMovingGroundSpeed(currentSpeed);
            }
            else
            {
                _wasRunning = false;
                return GetSlowdownGroundSpeed(currentSpeed);
            }
        }

        private float GetMovingGroundSpeed(float currentSpeed)
        {
            if (IsRunning)
            {
                return GetRunningSpeed(currentSpeed);
            }
            else if (_wasRunning && !IsMovingInOppositeDirection(currentSpeed))
            {
                _wasRunning = false;
                return Direction * WalkSpeed;
            }
            else
            {
                _wasRunning = false;
                return GetWalkingSpeed(currentSpeed);
            }
        }

        private float GetRunningSpeed(float currentSpeed)
        {
            if (IsMovingInOppositeDirection(currentSpeed) && Mathf.Abs(currentSpeed) > _skidTurnaroundSpeed)
            {
                IsSkidding = true;
                return GetSkiddingSpeed(currentSpeed);
            }
            else
            {
                IsSkidding = false;
                float newSpeed = Mathf.Lerp(currentSpeed, Direction * RunSpeed, _runAcceleration);
                return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * RunSpeed, Mathf.Abs(Direction) * RunSpeed);
            }
        }

        private float GetWalkingSpeed(float currentSpeed)
        {
            if (IsMovingInOppositeDirection(currentSpeed) && Mathf.Abs(currentSpeed) > _skidTurnaroundSpeed)
            {
                IsSkidding = true;
                return GetSkiddingSpeed(currentSpeed);
            }
            else
            {
                IsSkidding = false;
                float newSpeed = Mathf.Lerp(currentSpeed, Direction * WalkSpeed, _walkAcceleration);
                return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * WalkSpeed, Mathf.Abs(Direction) * WalkSpeed);
            }
        }

        private float GetSlowdownGroundSpeed(float currentSpeed)
        {
            if (IsSkidding && !IsCrouched)
            {
                float skiddingSpeed = GetSkiddingSpeed(currentSpeed);
                if (FloatOperator.EqualityComparisonWithTolerance(skiddingSpeed, 0.0f, 0.01f))
                {
                    IsSkidding = false;
                }
                return skiddingSpeed;
            }
            else
            {
                return Mathf.Lerp(currentSpeed, 0.0f, _releaseDeceleration);
            }
        }

        private bool IsMovingInOppositeDirection(float currentSpeed)
        {
            return Mathf.Sign(currentSpeed) != Mathf.Sign(Direction);
        }

        private float GetSkiddingSpeed(float currentSpeed)
        {
            return Mathf.Lerp(currentSpeed, 0.0f, _skiddingDeceleration);
        }

        private float GetAirSpeed(float currentSpeed)
        {
            if (Direction != 0.0f)
            {
                return GetMovingAirSpeed(currentSpeed);
            }
            else
            {
                return currentSpeed;
            }
        }

        private float GetMovingAirSpeed(float currentSpeed)
        {
            if (IsMovingInOppositeDirection(currentSpeed))
            {
                return GetDecreasingAirSpeed(currentSpeed);
            }
            else
            {
                return GetIncreasingAirSpeed(currentSpeed);
            }
        }

        private float GetDecreasingAirSpeed(float currentSpeed)
        {
            if (Mathf.Abs(currentSpeed) >= WalkSpeed)
            {
                if (Mathf.Abs(_speedBeforeJump) < WalkSpeed)
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * WalkSpeed, _fastAirDeceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * WalkSpeed, Mathf.Abs(Direction) * WalkSpeed);
                }
                else
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * RunSpeed, _fastAirDeceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * RunSpeed, Mathf.Abs(Direction) * RunSpeed);
                }
            }
            else if (Mathf.Abs(_speedBeforeJump) >= _slowAirDecelerationSpeedLimit)
            {
                float newSpeed = Mathf.Lerp(currentSpeed, Direction * RunSpeed, _mediumAirDeceleration);
                return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * RunSpeed, Mathf.Abs(Direction) * RunSpeed);
            }
            else
            {
                if (Mathf.Abs(_speedBeforeJump) < WalkSpeed)
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * WalkSpeed, _slowAirDeceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * WalkSpeed, Mathf.Abs(Direction) * WalkSpeed);
                }
                else
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * RunSpeed, _slowAirDeceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * RunSpeed, Mathf.Abs(Direction) * RunSpeed);
                }
            }
        }

        private float GetIncreasingAirSpeed(float currentSpeed)
        {
            if (Mathf.Abs(currentSpeed) < WalkSpeed)
            {
                if (Mathf.Abs(_speedBeforeJump) < WalkSpeed)
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * WalkSpeed, _slowAirAcceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * WalkSpeed, Mathf.Abs(Direction) * WalkSpeed);
                }
                else
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * RunSpeed, _slowAirAcceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * RunSpeed, Mathf.Abs(Direction) * RunSpeed);
                }
            }
            else
            {
                if (Mathf.Abs(_speedBeforeJump) < WalkSpeed)
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * WalkSpeed, _fastAirAcceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * WalkSpeed, Mathf.Abs(Direction) * WalkSpeed);
                }
                else
                {
                    float newSpeed = Mathf.Lerp(currentSpeed, Direction * RunSpeed, _fastAirAcceleration);
                    return Mathf.Clamp(newSpeed, -Mathf.Abs(Direction) * RunSpeed, Mathf.Abs(Direction) * RunSpeed);
                }
            }
        }

        public override void StartRunning()
        {
            IsRunning = true;
            _runReleaseTimerReference.Stop();
        }

        public override void StopRunning()
        {
            _runReleaseTimerReference.Start(_runReleaseTime);
        }

        public void OnRunReleaseTimerTimeout()
        {
            IsRunning = false;
            _wasRunning = true;
        }

        public override void StartCrouching()
        {
            IsCrouched = true;
        }

        public override void StopCrouching()
        {
            IsCrouched = false;
        }
    }
}