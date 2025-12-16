using Game.Buses;
using Game.Player;
using Game.Projectiles;
using Godot;

namespace Game.Enemies
{

    public class Castanea : Enemy, Jumpable, Burnable, Perishable
    {
        public int PerishPoints
        {
            get { return PERISH_POINTS; }
        }

        public AudioStreamPlayer SquishSoundPlayerReference
        {
            get { return _squishSoundPlayerReference; }
        }

        public AudioStreamPlayer DeathSoundPlayerReference
        {
            get { return _deathSoundPlayerReference; }
        }

        [Export]
        private NodePath _movementPath;
        private BasicMovement _movementReference;
        [Export]
        private NodePath _squishSoundPlayerPath;
        private AudioStreamPlayer _squishSoundPlayerReference;
        [Export]
        private NodePath _deathSoundPlayerPath;
        private AudioStreamPlayer _deathSoundPlayerReference;
        private float _deathBounceForce = -250.0f;
        [Export]
        private int[] _pointsChain;
        private const int PERISH_POINTS = 100;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
            _movementReference.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _movementReference = GetNode<BasicMovement>(_movementPath);
            _squishSoundPlayerReference = GetNode<AudioStreamPlayer>(_squishSoundPlayerPath);
            _deathSoundPlayerReference = GetNode<AudioStreamPlayer>(_deathSoundPlayerPath);
        }

        public override void EnableEnemy()
        {
            EnemyVisualReference.FlipV = false;
            EnemyVisualReference.Show();
            EnemyHitboxReference.SetDeferred("disabled", false);
            PhysicalHitboxReference.SetDeferred("disabled", false);
        }

        public void Squish(Vito jumpingPlayer)
        {
            AwardJumpingPoints(jumpingPlayer);
            jumpingPlayer.Bounce(EnemyHitboxAreaReference);
            EnemyVisualReference.Play("squish");
            EnemyHitboxReference.SetDeferred("disabled", true);
            _movementReference.ShouldMove = false;
            SquishSoundPlayerReference.Play();
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
            PointsTextFactory.CreatePointTextFromEnemy(PERISH_POINTS, GlobalPosition);
            Perish();
        }

        public void Perish()
        {
            PointsEventBus.Instance.EmitSignal("PointsGained", PERISH_POINTS);
            EnemyHitboxReference.SetDeferred("disabled", true);
            PhysicalHitboxReference.SetDeferred("disabled", true);
            EnemyVisualReference.FlipV = true;
            _movementReference.Speed = 0.0f;
            _movementReference.Velocity = new Vector2(0.0f, _deathBounceForce);
            _movementReference.ShouldMove = false;
            DeathSoundPlayerReference.Play();
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
                PointsTextFactory.CreatePointTextFromEnemy(PERISH_POINTS, GlobalPosition);
                Perish();
            }
            else if (area.IsInGroup("death_pit"))
            {
                DisableEnemy();
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
            EnemyVisualReference.Stop();
            EnemyVisualReference.Hide();
            EnemyHitboxReference.SetDeferred("disabled", true);
            PhysicalHitboxReference.SetDeferred("disabled", true);
        }

        private void HandlePlayerCollision(Node playerNode)
        {
            if (playerNode is Vito vito)
            {
                if (vito.IsInvincible)
                {
                    PointsTextFactory.CreatePointTextFromEnemy(PERISH_POINTS, GlobalPosition);
                    Perish();
                }
                else if (vito.GetVelocityVector().y > 0 || vito.IsHittingEnemyAbove())
                {
                    Squish(vito);
                }
                else if (vito.Damageable)
                {
                    vito.TakeDamage();
                }
            }
        }

        public override void OnScreenEntered()
        {
            _movementReference.ShouldMove = true;
            _movementReference.ShouldFall = true;
            EnemyVisualReference.Play("walk");
        }

        public override void OnScreenExited()
        {
            if (_movementReference.MovementDirection == Direction.Right)
            {
                return;
            }
            DisableEnemy();
        }

        public void OnAnimationFinished()
        {
            if (EnemyVisualReference.Animation != "squish")
            {
                return;
            }
            DisableEnemy();
        }
    }
}