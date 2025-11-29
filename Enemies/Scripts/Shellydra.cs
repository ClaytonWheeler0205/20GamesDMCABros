using Game.Buses;
using Game.Player;
using Game.Projectiles;
using Godot;
using Util.ExtensionMethods;

namespace Game.Enemies
{

    public class Shellydra : Enemy, Jumpable, Burnable, Perishable
    {
        public CollisionShape2D EnemyHitbox
        {
            get { return _hitboxReference; }
        }
        public int PerishPoints
        {
            get { return _perishPoints; }
        }

        [Export]
        private NodePath _hitboxPath;
        private CollisionShape2D _hitboxReference;
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
        private NodePath _physicalHitboxPath;
        private CollisionShape2D _physicalHitboxReference;
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
        [Export]
        private int _shellKickPoints = 400;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            _movementReference.BodyToMove = this;
            _shellMovementReference.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _hitboxReference = GetNode<CollisionShape2D>(_hitboxPath);
            _shellHitboxReference = GetNode<CollisionShape2D>(_shellHitBoxPath);
            _movementReference = GetNode<BasicMovement>(_movementPath);
            _shellMovementReference = GetNode<BasicMovement>(_shellMovementPath);
            _physicalHitboxReference = GetNode<CollisionShape2D>(_physicalHitboxPath);
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
            _squishSoundReference.Play();
        }

        private async void HideInShell()
        {
            EnemyVisualReference.Stop();
            EnemyVisualReference.Animation = "shell";
            EnemyVisualReference.Offset = new Vector2(0, 5);
            _inShell = true;
            _movementReference.ShouldMove = false;
            _hitboxReference.SetDeferred("disabled", true);
            await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
            _shellHitboxReference.SetDeferred("disabled", false);
        }

        private void HandleShellJumpPoints(Vito jumpingPlayer)
        {
            if (jumpingPlayer.JumpChainCount == 0)
            {
                PointsTextFactory.CreatePointTextFromEnemy(_shellKickPoints, GlobalPosition);
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
            EnemyHitbox.SetDeferred("disabled", true);
            _physicalHitboxReference.SetDeferred("disabled", true);
            EnemyVisualReference.Stop();
            EnemyVisualReference.Animation = "shell";
            EnemyVisualReference.FlipV = true;
            _movementReference.Speed = 0.0f;
            _movementReference.Velocity = new Vector2(0.0f, _deathBounceForce);
            _movementReference.ShouldMove = true;
            _deathSoundReference.Play();
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
                        PointsTextFactory.CreatePointTextFromEnemy(_shellKickPoints, GlobalPosition);
                        KickShell(vito.GlobalPosition.x);
                        return;
                    }
                    GD.Print("Took damage!");
                }
            }
        }

        private void KickShell(float playerXPosition)
        {
            if (playerXPosition > GlobalPosition.x)
            {
                _shellMovementReference.FlipDirection();
            }
            _movementReference.ShouldFall = false;
            _shellMovementReference.ShouldMove = true;
            _shellMovementReference.ShouldFall = true;
            _kickSoundReference.Play();
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
                Perish();
            }
            else if (area.IsInGroup("death_pit"))
            {
                this.SafeQueueFree();
            }
            else if (area.IsInGroup("enemy"))
            {
                HandleEnemyCollision(area);
            }
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
            this.SafeQueueFree();
        }

        private bool ShouldStayInLevel()
        {
            return (_movementReference.MovementDirection == Direction.Right && _movementReference.ShouldMove) || (_shellMovementReference.MovementDirection == Direction.Right && _shellMovementReference.ShouldMove) || !_inShell;
        }

        public void OnDirectionFlipped()
        {
            EnemyVisualReference.FlipH = !EnemyVisualReference.FlipH;
        }
    }
}