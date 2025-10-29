using Game.Player;
using Godot;

namespace Game.Blocks
{
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
        private Node2D _blockVisualReference;
        protected Node2D BlockVisualReference
        {
            get { return _blockVisualReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _hitboxReference = GetNode<BlockHitbox>(_hitboxPath);
            _bounceAnimationReference = GetNode<AnimationPlayer>(_bounceAnimationPath);
            _interactionHitBoxReference = GetNode<CollisionShape2D>(_interactionHitBoxPath);
            _blockVisualReference = GetNode<Node2D>(_blockVisualPath);
        }
    }
}