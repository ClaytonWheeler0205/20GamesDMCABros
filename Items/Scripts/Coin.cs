using Godot;
using Util.ExtensionMethods;

namespace Game.Items
{

    public class Coin : Node
    {
        [Export]
        private NodePath _coinVisualPath;
        private AnimatedSprite _coinVisualReference;
        [Export]
        private NodePath _coinCollisionPath;
        private CollisionShape2D _coinCollisionReference;
        [Export]
        private NodePath _coinCollectSoundPath;
        private AudioStreamPlayer _coinCollectSoundReference;

        public override void _Ready()
        {
            SetNodeReferences();
            _coinVisualReference.Play();
        }

        private void SetNodeReferences()
        {
            _coinVisualReference = GetNode<AnimatedSprite>(_coinVisualPath);
            _coinCollisionReference = GetNode<CollisionShape2D>(_coinCollisionPath);
            _coinCollectSoundReference = GetNode<AudioStreamPlayer>(_coinCollectSoundPath);
        }

        public void OnBodyEntered(Node body)
        {
            if (body.IsInGroup("player"))
            {
                _coinVisualReference.Visible = false;
                _coinCollisionReference.SetDeferred("disabled", true);
                _coinCollectSoundReference.Play();
                //TODO: Update the player's score
            }
        }

        public void OnSoundFinished()
        {
            this.SafeQueueFree();
        }
    }
}