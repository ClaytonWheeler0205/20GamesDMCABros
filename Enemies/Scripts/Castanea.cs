using Game.Player;
using Godot;
using Util.ExtensionMethods;

namespace Game.Enemies
{

    public class Castanea : Enemy, Jumpable
    {
        [Export]
        private NodePath _movementPath;
        private BasicMovement _movementReference;
        [Export]
        private NodePath _hitboxPath;
        private CollisionShape2D _hitboxReference;
        [Export]
        private NodePath _squishSoundPath;
        private AudioStreamPlayer _squishSoundReference;

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
        }

        public void Squish(Vito jumpingPlayer)
        {
            jumpingPlayer.Bounce();
            EnemyVisualReference.Play("squish");
            _hitboxReference.SetDeferred("disabled", true);
            _movementReference.ShouldMove = false;
            _squishSoundReference.Play();
        }

        public override void OnBodyEntered(Node body)
        {
            if (body.IsInGroup("player"))
            {
                HandlePlayerCollision(body);
            }
            else if (body.IsInGroup("fire"))
            {
                GD.Print("Burned!");
            }
            else if (body.IsInGroup("ice"))
            {
                GD.Print("Frozen!");
            }
        }

        private void HandlePlayerCollision(Node playerNode)
        {
            if (playerNode is Vito vito)
            {
                if (vito.IsHittingEnemyAbove())
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