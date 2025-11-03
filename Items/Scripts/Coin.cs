using Game.Buses;
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
        private const int COIN_POINT_VALUE = 200;

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
                CoinEventBus.Instance.EmitSignal("CoinCollected");
                PointsEventBus.Instance.EmitSignal("PointsGained", COIN_POINT_VALUE);
            }
        }

        public void OnSoundFinished()
        {
            this.SafeQueueFree();
        }
    }
}