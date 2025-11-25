using Game.Buses;
using Game.Player;
using Game.Projectiles;
using Godot;
using Util.ExtensionMethods;

namespace Game.Enemies
{

    public class Castanea : Enemy, Jumpable, Burnable, Perishable
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
        private NodePath _movementPath;
        private BasicMovement _movementReference;
        [Export]
        private NodePath _hitboxPath;
        private CollisionShape2D _hitboxReference;
        [Export]
        private NodePath _physicalHitboxPath;
        private CollisionShape2D _physicalHitboxReference;
        [Export]
        private NodePath _squishSoundPath;
        private AudioStreamPlayer _squishSoundReference;
        [Export]
        private NodePath _deathSoundPath;
        private AudioStreamPlayer _deathSoundReference;
        private float _deathBounceForce = -250.0f;
        [Export]
        private int _perishPoints = 100;
        [Export]
        private int[] _pointsChain;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            _movementReference.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<BasicMovement>(_movementPath);
            _hitboxReference = GetNode<CollisionShape2D>(_hitboxPath);
            _squishSoundReference = GetNode<AudioStreamPlayer>(_squishSoundPath);
            _physicalHitboxReference = GetNode<CollisionShape2D>(_physicalHitboxPath);
            _deathSoundReference = GetNode<AudioStreamPlayer>(_deathSoundPath);
        }

        public void Squish(Vito jumpingPlayer)
        {
            AwardJumpingPoints(jumpingPlayer);
            jumpingPlayer.Bounce(EnemyHitboxAreaReference);
            EnemyVisualReference.Play("squish");
            EnemyHitbox.SetDeferred("disabled", true);
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
            EnemyVisualReference.FlipV = true;
            _movementReference.Speed = 0.0f;
            _movementReference.Velocity = new Vector2(0.0f, _deathBounceForce);
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
            else if (body.IsInGroup("ice"))
            {
                GD.Print("Frozen!");
            }
            else if (body is Fireball fireball)
            {
                fireball.DestroyFireball();
                Burn();
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
                    GD.Print("Took damage!");
                }
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

        public void OnAnimationFinished()
        {
            if (EnemyVisualReference.Animation != "squish")
            {
                return;
            }
            this.SafeQueueFree();
        }
    }
}