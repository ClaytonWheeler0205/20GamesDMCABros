using Game.Buses;
using Game.Debug;
using Game.Projectiles;
using Godot;
using System.Collections.Generic;
using Game.Enemies;
using System.Runtime.Serialization.Formatters;

namespace Game.Player
{

    public class VitoImpl : Vito
    {
        private Vector2 _velocity = new Vector2();
        private Dictionary<string, CollisionShape2D> _physicalCollisions;
        private Dictionary<string, CollisionShape2D> _jumpInteractionCollisions;
        private List<RayCast2D> _enemyRayCasts;

        [Export]
        private NodePath _debugPath;
        private VitoDebug _debug;
        private bool _shouldMove = true;
        private float _deathBounceForce = -100.0f;

        public override void _Ready()
        {
            base._Ready();
            SetupCollisionDictionaries();
            SetupNodeConnections();
            SetupEnemyDetection();
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

        private void SetupEnemyDetection()
        {
            _enemyRayCasts = new List<RayCast2D>(EnemyDetectorsReference.GetChildCount());
            foreach (Node node in EnemyDetectorsReference.GetChildren())
            {
                if (node is RayCast2D ray)
                {
                    _enemyRayCasts.Add(ray);
                }
            }
        }

        private void SetupNodeConnections()
        {
            PowerupEventBus.Instance.Connect("MushroomCollected", this, nameof(OnMushroomCollected));
            PowerupEventBus.Instance.Connect("FlowerCollected", this, nameof(OnFlowerCollected));
            PowerupEventBus.Instance.Connect("StarCollected", this, nameof(OnStarCollected));
        }

        public override void _Process(float delta)
        {
            if (_velocity.x > 0.0f)
            {
                FireballPoolReference.Position = RightFireballSpawn;
            }
            else if (_velocity.x < 0.0f)
            {
                FireballPoolReference.Position = LeftFireballSpawn;
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_shouldMove)
            {
                ApplyVerticalForce(delta);
                ApplyHorizontalForce();
                AttemptCornerCorrection(3);
                _velocity = MoveAndSlide(_velocity, Vector2.Up);
                JumpHitDataReference.VerticalVelocity = _velocity.y;
                JumpHitDataReference.YPosition = GlobalPosition.y;
            }
            if (IsOnFloor())
            {
                JumpHitDataReference.HasHitBlock = false;
            }
            if (!IsHittingEnemyAbove() && IsOnFloor())
            {
                JumpChainCount = 0;
            }
            _debug.DisplayGround(IsOnFloor());
            if (_velocity.y >= 0)
            {
                EnableEnemyRays();
            }
            else
            {
                DisableEnemyRays();
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

        private void ApplyVerticalForce(float delta)
        {
            _velocity.y += JumpComponentReference.GetGravity(_velocity.y) * delta;
            if (_velocity.y > JumpComponentReference.TerminalVelocity)
            {
                _velocity.y = JumpComponentReference.TerminalVelocity;
            }
        }

        private void ApplyHorizontalForce()
        {
            _velocity.x = MovementComponentReference.GetMovementSpeed(_velocity.x);
            if (Mathf.Abs(_velocity.x) < 10.0f)
            {
                _velocity.x = 0.0f;
            }
        }

        public override bool IsHittingEnemyAbove()
        {
            foreach (RayCast2D ray in _enemyRayCasts)
            {
                if (ray.IsColliding())
                {
                    return true;
                }
            }
            return false;
        }

        private void EnableEnemyRays()
        {
            foreach (RayCast2D ray in _enemyRayCasts)
            {
                ray.Enabled = true;
            }
        }

        private void DisableEnemyRays()
        {
            foreach (RayCast2D ray in _enemyRayCasts)
            {
                ray.Enabled = false;
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
                        CanThrow = true;
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
                        CanThrow = false;
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
            CanThrow = true;
        }

        public override void ShootFireball()
        {
            //TODO: fix the bug with the fireball factory where its position can be above or below the usual position
            if (!HasFlower || !CanThrow)
            {
                return;
            }
            Fireball fireBallToShoot = FireballPoolReference.GetFireball();
            if (fireBallToShoot == null)
            {
                return;
            }
            fireBallToShoot.GlobalPosition = FireballPoolReference.GlobalPosition;
            if (FireballPoolReference.Position == RightFireballSpawn)
            {
                fireBallToShoot.MovementDirection = Direction.Right;
            }
            else
            {
                fireBallToShoot.MovementDirection = Direction.Left;
            }
            fireBallToShoot.Enable();
            PlayerEventBus.Instance.EmitSignal("FireballThrown");
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

        public void OnFlowerCollected()
        {
            if (GlobalPlayerData.PlayerSize == Size.Small)
            {
                PowerupEventBus.Instance.EmitSignal("MushroomCollected");
                return;
            }
            if (HasFlower)
            {
                return;
            }
            GrabFlower();
        }

        private void GrabFlower()
        {
            HasFlower = true;
            PaletteAnimatorReference.PlaybackSpeed = 1.0f;
            PaletteAnimatorReference.Play("fire_transform");
            GetTree().Paused = true;
        }

        public void OnStarCollected()
        {
            IsInvincible = true;
            PaletteAnimatorReference.PlaybackSpeed = 2.0f;
            PaletteAnimatorReference.Play("invincibility_flash");
            StarComponentReference.StartTimers();
        }

        public void OnInvincibilityTimeTimeout()
        {
            IsInvincible = false;
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

        public override void Bounce(Area2D enemyBouncedOnHitbox)
        {
            _velocity.y = JumpComponentReference.BouncePower;
            JumpChainCount++;
            if (enemyBouncedOnHitbox.IsInGroup("castanea"))
            {
                CheckForMultipleCastaneaStomps(enemyBouncedOnHitbox);
            }
        }

        private void CheckForMultipleCastaneaStomps(Area2D currentEnemyArea)
        {
            foreach (RayCast2D ray in _enemyRayCasts)
            {
                if (ray.GetCollider() is Area2D enemyArea)
                {
                    if (IsValidForExtraJumpPoints(currentEnemyArea, enemyArea))
                    {
                        JumpChainCount++;
                    }
                }
            }
        }

        private bool IsValidForExtraJumpPoints(Area2D currentEnemyArea, Area2D otherEnemyArea)
        {
            return otherEnemyArea.IsInGroup("castanea") && currentEnemyArea != otherEnemyArea && JumpChainCount < 3;
        }

        public override void TakeDamage()
        {
            if (GlobalPlayerData.PlayerSize == Size.Big)
            {
                Shrink();
            }
            else
            {
                Die();
            }
        }

        private void Shrink()
        {
            Damageable = false;
            HasFlower = false;
            GlobalPlayerData.PlayerSize = Size.Small;
            PaletteComponentReference.SetPlayerColor(0);
            SuperPlayerVisualReference.ToggleAnimation();
            SmallPlayerVisualReference.ToggleAnimation();
            _physicalCollisions["small"].SetDeferred("disabled", false);
            _physicalCollisions["super"].SetDeferred("disabled", true);
            _physicalCollisions["crouched"].SetDeferred("disabled", true);
            _jumpInteractionCollisions["small"].SetDeferred("disabled", false);
            _jumpInteractionCollisions["super"].SetDeferred("disabled", true);
            _jumpInteractionCollisions["crouched"].SetDeferred("disabled", true);
            ShrinkSoundPlayerReference.Play();
            InvincibilityFlashPlayerReference.Play("invincibility_flash");
            IncinvibilityFlashTimerReference.Start();
            PlayerEventBus.Instance.EmitSignal("DamageTaken");
        }

        private async void Die()
        {
            PauseMode = PauseModeEnum.Process;
            _shouldMove = false;
            PlayerEventBus.Instance.EmitSignal("PlayerDied");
            _physicalCollisions["small"].SetDeferred("disabled", true);
            await ToSignal(GetTree().CreateTimer(0.7f), "timeout");
            DeathAnimationPlayerReference.Play("death");
        }

        public override void OnInvincibilityFlashTimeTimeout()
        {
            InvincibilityFlashPlayerReference.Stop();
            Visible = true;
            Damageable = true;
        }
    }
}