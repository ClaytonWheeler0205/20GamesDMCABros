using Game.Player;
using Godot;

namespace Game.Blocks
{

    public enum Item
    {
        None,
        Powerup,
        Life,
        Star
    }

    public abstract class Block : Node2D
    {
        [Export]
        private NodePath _hitboxPath;
        private BlockHitbox _hitboxReference;
        protected BlockHitbox HitBoxReference
        {
            get { return _hitboxReference; }
        }
        [Export]
        private NodePath _bounceAnimationPath;
        private AnimationPlayer _bounceAnimationReference;
        protected AnimationPlayer BounceAnimationReference
        {
            get { return _bounceAnimationReference; }
        }
        [Export]
        private NodePath _interactionHitBoxPath;
        private CollisionShape2D _interactionHitBoxReference;
        protected CollisionShape2D InteractionHitBoxReference
        {
            get { return _interactionHitBoxReference; }
        }
        [Export]
        private NodePath _blockVisualPath;
        private Sprite _blockVisualReference;
        protected Sprite BlockVisualReference
        {
            get { return _blockVisualReference; }
        }
        [Export]
        private Item _itemInBlock = Item.None;
        protected Item ItemInBlock
        {
            get { return _itemInBlock; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _hitboxReference = GetNode<BlockHitbox>(_hitboxPath);
            _bounceAnimationReference = GetNode<AnimationPlayer>(_bounceAnimationPath);
            _interactionHitBoxReference = GetNode<CollisionShape2D>(_interactionHitBoxPath);
            _blockVisualReference = GetNode<Sprite>(_blockVisualPath);
        }

        private void SetNodeConnections()
        {
            _hitboxReference.Connect("BlockHitByPlayer", this, nameof(OnBlockHitByPlayer));
        }

        public abstract void OnBlockHitByPlayer(Size playerSize);
    }
}