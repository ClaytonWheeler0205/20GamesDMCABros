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
        private NodePath _movementPath;
        private BasicMovement _movementReference;
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
        private NodePath _shellBounceSoundPath;
        private AudioStreamPlayer _shellBounceSoundReference;
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

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            _movementReference.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _hitboxReference = GetNode<CollisionShape2D>(_hitboxPath);
            _movementReference = GetNode<BasicMovement>(_movementPath);
            _physicalHitboxReference = GetNode<CollisionShape2D>(_physicalHitboxPath);
            _squishSoundReference = GetNode<AudioStreamPlayer>(_squishSoundPath);
            _deathSoundReference = GetNode<AudioStreamPlayer>(_deathSoundPath);
            _kickSoundReference = GetNode<AudioStreamPlayer>(_kickSoundPath);
            _shellBounceSoundReference = GetNode<AudioStreamPlayer>(_shellBounceSoundPath);
            _shellHideTimerReference = GetNode<Timer>(_shellHideTimerPath);
            _shellShakeTimerReference = GetNode<Timer>(_shellShakeTimerPath);
        }

        public void Squish(Vito jumpingPlayer)
        {
            AwardJumpingPoints(jumpingPlayer);
            jumpingPlayer.Bounce(EnemyHitboxAreaReference);
            EnemyVisualReference.Stop();
            EnemyVisualReference.Animation = "shell";
            EnemyVisualReference.Offset = new Vector2(0, 5);
            _inShell = true;
            _movementReference.ShouldMove = false;
            _squishSoundReference.Play();
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
                    if (_inShell)
                    {
                        KickShell(vito.GlobalPosition);
                        return;
                    }
                    GD.Print("Took damage!");
                }
            }
        }

        private void KickShell(Vector2 playerPosition)
        {
            GD.Print("Kick shell!");
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
        }

        public override void OnScreenEntered()
        {
            _movementReference.ShouldMove = true;
            EnemyVisualReference.Play("walk");
        }

        public override void OnScreenExited()
        {
            if (_movementReference.MovementDirection == Direction.Left)
            {
                return;
            }
            this.SafeQueueFree();
        }
    }
}