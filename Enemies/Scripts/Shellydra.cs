using Game.Buses;
using Game.Player;
using Game.Projectiles;
using Godot;

namespace Game.Enemies
{

    public class Shellydra : Enemy, Jumpable, Burnable, Perishable
    {
        public int PerishPoints
        {
            get { return _perishPoints; }
        }

        public AudioStreamPlayer SquishSoundPlayerReference
        {
            get { return _squishSoundReference; }
        }

        public AudioStreamPlayer DeathSoundPlayerReference
        {
            get { return _deathSoundReference; }
        }

        [Export]
        private NodePath _shellHitBoxPath;
        private CollisionShape2D _shellHitboxReference;
        [Export]
        private NodePath _movementPath;
        private BasicMovement _movementReference;
        [Export]
        private NodePath _shellMovementPath;
        private BasicMovement _shellMovementReference;
        [Export]
        private NodePath _squishSoundPath;
        private AudioStreamPlayer _squishSoundReference;
        [Export]
        private NodePath _deathSoundPath;
        private AudioStreamPlayer _deathSoundReference;
        [Export]
        private NodePath _kickSoundPath;
        private AudioStreamPlayer _kickSoundReference;
        [Export]
        private NodePath _shellHideTimerPath;
        private Timer _shellHideTimerReference;
        [Export]
        private NodePath _shellShakeTimerPath;
        private Timer _shellShakeTimerReference;
        private float _deathBounceForce = -250f;
        [Export]
        private int _perishPoints = 200;
        [Export]
        private int[] _pointsChain;
        private bool _inShell = false;
        [Export]
        private int[] _shellPointsChain;
        private int _shellChainCount = 0;
        private const int SHELL_JUMP_CHAIN_MINIMUM = 3;
        private const int SHELL_KICK_POINTS = 400;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            _movementReference.BodyToMove = this;
            _shellMovementReference.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _shellHitboxReference = GetNode<CollisionShape2D>(_shellHitBoxPath);
            _movementReference = GetNode<BasicMovement>(_movementPath);
            _shellMovementReference = GetNode<BasicMovement>(_shellMovementPath);
            _squishSoundReference = GetNode<AudioStreamPlayer>(_squishSoundPath);
            _deathSoundReference = GetNode<AudioStreamPlayer>(_deathSoundPath);
            _kickSoundReference = GetNode<AudioStreamPlayer>(_kickSoundPath);
            _shellHideTimerReference = GetNode<Timer>(_shellHideTimerPath);
            _shellShakeTimerReference = GetNode<Timer>(_shellShakeTimerPath);
        }

        public void Squish(Vito jumpingPlayer)
        {
            if (_inShell)
            {
                HandleShellJumpPoints(jumpingPlayer);
                HandleShellJump(jumpingPlayer.GlobalPosition.x);
                jumpingPlayer.Bounce(EnemyHitboxAreaReference);
                return;
            }
            AwardJumpingPoints(jumpingPlayer);
            jumpingPlayer.Bounce(EnemyHitboxAreaReference);
            HideInShell();
            if (!IsOnFloor())
            {
                EnemyVisualReference.FlipV = true;
            }
            SquishSoundPlayerReference.Play();
        }

        private async void HideInShell()
        {
            EnemyVisualReference.Stop();
            EnemyVisualReference.Animation = "shell";
            EnemyVisualReference.Offset = new Vector2(0, 5);
            _movementReference.ShouldMove = false;
            _movementReference.ShouldFall = false;
            _shellMovementReference.ShouldFall = true;
            EnemyHitboxReference.SetDeferred("disabled", true);
            _shellHideTimerReference.Start();
            _shellShakeTimerReference.Start();
            _inShell = true;
            await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
            _shellHitboxReference.SetDeferred("disabled", false);
        }

        private void HandleShellJumpPoints(Vito jumpingPlayer)
        {
            if (jumpingPlayer.JumpChainCount == 0)
            {
                PointsTextFactory.CreatePointTextFromEnemy(SHELL_KICK_POINTS, GlobalPosition);
            }
            else if (jumpingPlayer.JumpChainCount <= SHELL_JUMP_CHAIN_MINIMUM)
            {
                PointsTextFactory.CreatePointTextFromEnemy(_pointsChain[SHELL_JUMP_CHAIN_MINIMUM], GlobalPosition);
                jumpingPlayer.JumpChainCount = SHELL_JUMP_CHAIN_MINIMUM;
            }
            else
            {
                AwardJumpingPoints(jumpingPlayer);
            }
        }

        private void HandleShellJump(float jumpingPlayerXPosition)
        {
            if (_shellMovementReference.ShouldMove)
            {
                _shellHideTimerReference.Start();
                _shellShakeTimerReference.Start();
                _shellMovementReference.ShouldMove = false;
                _squishSoundReference.Play();
            }
            else
            {
                KickShell(jumpingPlayerXPosition);
            }
        }

        public void AwardJumpingPoints(Vito jumpingPlayer)
        {
            if (jumpingPlayer.JumpChainCount >= _pointsChain.Length)
            {
                PointsTextFactory.CreatePointTextFromEnemy(0, GlobalPosition);
                return;
            }
            PointsTextFactory.CreatePointTextFromEnemy(_pointsChain[jumpingPlayer.JumpChainCount], GlobalPosition);
        }

        public void Burn()
        {
            Perish();
        }

        public void Perish()
        {
            PointsEventBus.Instance.EmitSignal("PointsGained", _perishPoints);
            PointsTextFactory.CreatePointTextFromEnemy(_perishPoints, GlobalPosition);
            EnemyHitboxReference.SetDeferred("disabled", true);
            _shellHitboxReference.SetDeferred("disabled", true);
            PhysicalHitboxReference.SetDeferred("disabled", true);
            EnemyVisualReference.Stop();
            EnemyVisualReference.Frame = 0;
            EnemyVisualReference.Animation = "shell";
            EnemyVisualReference.FlipV = true;
            _movementReference.Velocity = new Vector2(0.0f, _deathBounceForce);
            _movementReference.ShouldMove = false;
            _movementReference.ShouldFall = true;
            _shellMovementReference.ShouldMove = false;
            _shellMovementReference.ShouldFall = false;
            DeathSoundPlayerReference.Play();
        }

        public override void EnableEnemy()
        {
            Show();
            EnemyHitboxReference.SetDeferred("disabled", false);
            PhysicalHitboxReference.SetDeferred("disabled", false);
        }

        public override void OnBodyEntered(Node body)
        {
            if (!EnemyScreenDetectorReference.IsOnScreen())
            {
                return;
            }
            if (body.IsInGroup("player"))
            {
                HandlePlayerCollision(body);
            }
            else if (body is Fireball fireball)
            {
                fireball.DestroyFireball();
                HandleFireballCollision(fireball);
            }
        }

        private void HandlePlayerCollision(Node playerNode)
        {
            if (playerNode is Vito vito)
            {
                if (vito.IsInvincible)
                {
                    Perish();
                }
                else if (vito.GetVelocityVector().y > 0 || vito.IsHittingEnemyAbove())
                {
                    Squish(vito);
                }
                else
                {
                    if (_inShell && !_shellMovementReference.ShouldMove)
                    {
                        PointsTextFactory.CreatePointTextFromEnemy(SHELL_KICK_POINTS, GlobalPosition);
                        KickShell(vito.GlobalPosition.x);
                        return;
                    }
                    if (!vito.Damageable)
                    {
                        return;
                    }
                    vito.TakeDamage();
                }
            }
        }

        private void KickShell(float playerXPosition)
        {
            if (ShouldFlipDirection(playerXPosition))
            {
                _shellMovementReference.FlipDirection();
            }
            _shellMovementReference.ShouldMove = true;
            _kickSoundReference.Play();
            EnemyVisualReference.Stop();
            EnemyVisualReference.Frame = 0;
            _shellHideTimerReference.Stop();
            _shellShakeTimerReference.Stop();
        }

        private bool ShouldFlipDirection(float playerXPosition)
        {
            return (playerXPosition > GlobalPosition.x && _shellMovementReference.MovementDirection == Direction.Right) || (playerXPosition <= GlobalPosition.x && _shellMovementReference.MovementDirection == Direction.Left);
        }

        private void HandleFireballCollision(Fireball fireball)
        {
            if (fireball.IsInGroup("fire"))
            {
                Burn();
            }
            else if (fireball.IsInGroup("ice"))
            {
                GD.Print("Frozen!");
            }
        }

        public override void OnAreaEntered(Area2D area)
        {
            if (area.IsInGroup("block_damage"))
            {
                HideInShell();
                _movementReference.Velocity = new Vector2(0, _deathBounceForce);
                EnemyVisualReference.FlipV = true;
            }
            else if (area.IsInGroup("death_pit"))
            {
                DisableEnemy();
            }
            else if (area.IsInGroup("enemy"))
            {
                HandleEnemyCollision(area);
            }
        }

        public override void DisableEnemy()
        {
            _movementReference.ShouldMove = false;
            _movementReference.ShouldFall = false;
            if (_movementReference.MovementDirection == Direction.Right)
            {
                _movementReference.FlipDirection();
            }
            if (_shellMovementReference.MovementDirection == Direction.Right)
            {
                _shellMovementReference.FlipDirection();
            }
            EnemyVisualReference.Stop();
            EnemyVisualReference.FlipH = false;
            Hide();
            EnemyHitboxAreaReference.SetDeferred("disabled", true);
            PhysicalHitboxReference.SetDeferred("disabled", true);
        }

        private void HandleEnemyCollision(Node enemyNode)
        {
            if (!_shellMovementReference.ShouldMove)
            {
                return;
            }
            if (enemyNode.GetParent() is Perishable perishableEnemy)
            {
                AwardShellPoints();
                perishableEnemy.Perish();
            }
        }

        private void AwardShellPoints()
        {
            if (_shellChainCount < _shellPointsChain.Length)
            {
                PointsTextFactory.CreatePointTextFromEnemy(_shellPointsChain[_shellChainCount], GlobalPosition);
                _shellChainCount++;
                return;
            }
            PointsTextFactory.CreatePointTextFromEnemy(0, GlobalPosition);
        }

        public override void OnScreenEntered()
        {
            _movementReference.ShouldMove = true;
            _movementReference.ShouldFall = true;
            EnemyVisualReference.Play("walk");
        }

        public override void OnScreenExited()
        {
            if (ShouldStayInLevel())
            {
                return;
            }
            DisableEnemy();
        }

        private bool ShouldStayInLevel()
        {
            return ((_movementReference.MovementDirection == Direction.Right && _movementReference.ShouldMove) || (_shellMovementReference.MovementDirection == Direction.Right && _shellMovementReference.ShouldMove)) && !_inShell;
        }

        public void OnDirectionFlipped()
        {
            EnemyVisualReference.FlipH = !EnemyVisualReference.FlipH;
        }

        public void OnShellHideTimeout()
        {

            _shellMovementReference.ShouldMove = false;
            _shellMovementReference.ShouldFall = false;
            _movementReference.ShouldMove = true;
            _movementReference.ShouldFall = true;
            if (_movementReference.MovementDirection != _shellMovementReference.MovementDirection)
            {
                _movementReference.FlipDirection();
            }
            if (_movementReference.MovementDirection == Direction.Left)
            {
                EnemyVisualReference.FlipH = false;
            }
            else
            {
                EnemyVisualReference.FlipH = true;
            }
            EnemyVisualReference.FlipV = false;
            EnemyVisualReference.Offset = Vector2.Zero;
            EnemyVisualReference.Play("walk");
            _inShell = false;
        }

        public void OnShellShakeTimeout()
        {
            EnemyVisualReference.Play("shell");
        }
    }
}