using Game.Buses;
using Game.Debug;
using Godot;
using System.Collections.Generic;

namespace Game.Player
{

    public class VitoImpl : Vito
    {
        private Vector2 _velocity = new Vector2();
        private Dictionary<string, CollisionShape2D> _physicalCollisions;
        private Dictionary<string, CollisionShape2D> _jumpInteractionCollisions;

        [Export]
        private NodePath _debugPath;
        private VitoDebug _debug;

        public override void _Ready()
        {
            base._Ready();
            SetupCollisionDictionaries();
            SetupNodeConnections();
            _debug = GetNode<VitoDebug>(_debugPath);
        }

        private void SetupCollisionDictionaries()
        {
            SetupPhysicalCollisionDictionary();
            SetupJumpInteractionDictionary();
        }

        private void SetupPhysicalCollisionDictionary()
        {
            _physicalCollisions = new Dictionary<string, CollisionShape2D>(3);
            foreach (Node node in GetChildren())
            {
                if (node is CollisionShape2D shape)
                {
                    if (node.Name.ToLower().Contains("small"))
                    {
                        _physicalCollisions.Add("small", shape);
                    }
                    else if (node.Name.ToLower().Contains("super"))
                    {
                        _physicalCollisions.Add("super", shape);
                    }
                    else if (node.Name.ToLower().Contains("crouched"))
                    {
                        _physicalCollisions.Add("crouched", shape);
                    }
                }
            }
        }

        private void SetupJumpInteractionDictionary()
        {
            _jumpInteractionCollisions = new Dictionary<string, CollisionShape2D>(3);
            foreach (Node node in JumpHitDataReference.GetChildren())
            {
                if (node is CollisionShape2D shape)
                {
                    if (node.Name.ToLower().Contains("small"))
                    {
                        _jumpInteractionCollisions.Add("small", shape);
                    }
                    else if (node.Name.ToLower().Contains("super"))
                    {
                        _jumpInteractionCollisions.Add("super", shape);
                    }
                    else if (node.Name.ToLower().Contains("crouched"))
                    {
                        _jumpInteractionCollisions.Add("crouched", shape);
                    }
                }
            }
        }

        private void SetupNodeConnections()
        {
            PowerupEventBus.Instance.Connect("MushroomCollected", this, nameof(OnMushroomCollected));
        }

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

        public override void StartCrouching()
        {
            if (IsOnFloor())
            {
                MovementComponentReference.StartCrouching();
                if (GlobalPlayerData.PlayerSize == Size.Big)
                {
                    if (MovementComponentReference.Direction != 0.0f)
                    {
                        if (_physicalCollisions["super"].Disabled)
                        {
                            _physicalCollisions["super"].SetDeferred("disabled", false);
                            _physicalCollisions["crouched"].SetDeferred("disabled", true);
                            _jumpInteractionCollisions["super"].SetDeferred("disabled", false);
                            _jumpInteractionCollisions["crouched"].SetDeferred("disabled", true);
                        }
                    }
                    else
                    {
                        if (!_physicalCollisions["super"].Disabled)
                        {
                            _physicalCollisions["super"].SetDeferred("disabled", true);
                            _physicalCollisions["crouched"].SetDeferred("disabled", false);
                            _jumpInteractionCollisions["super"].SetDeferred("disabled", true);
                            _jumpInteractionCollisions["crouched"].SetDeferred("disabled", false);
                        }
                    }
                }
            }
        }

        public override void StopCrouching()
        {
            MovementComponentReference.StopCrouching();
            if (GlobalPlayerData.PlayerSize == Size.Big)
            {
                if (_physicalCollisions["super"].Disabled)
                {
                    _physicalCollisions["super"].SetDeferred("disabled", false);
                    _physicalCollisions["crouched"].SetDeferred("disabled", true);
                    _jumpInteractionCollisions["super"].SetDeferred("disabled", false);
                    _jumpInteractionCollisions["crouched"].SetDeferred("disabled", true);
                }
            }
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

        public void OnMushroomCollected()
        {
            GrowBig();
        }

        public override Vector2 GetVelocityVector()
        {
            return _velocity;
        }

        public override void SetMovementDirection(float newDirection)
        {
            MovementComponentReference.Direction = newDirection;
            if (_debug.Visible)
            {
                _debug.DisplayDirection(MovementComponentReference.Direction);
            }
        }

        private void GrowBig()
        {
            if (GlobalPlayerData.PlayerSize == Size.Small)
            {
                GlobalPlayerData.PlayerSize = Size.Big;
                SmallPlayerVisualReference.ToggleAnimation();
                SuperPlayerVisualReference.ToggleAnimation();
                _physicalCollisions["small"].SetDeferred("disabled", true);
                _physicalCollisions["super"].SetDeferred("disabled", false);
                _jumpInteractionCollisions["small"].SetDeferred("disabled", true);
                _jumpInteractionCollisions["super"].SetDeferred("disabled", false);
            }
        }
    }
}