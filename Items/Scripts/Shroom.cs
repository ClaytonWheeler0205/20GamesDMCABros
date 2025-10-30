using Godot;

namespace Game.Items
{

    public abstract class Shroom : KinematicBody2D
    {
        [Export]
        private NodePath _shroomVisualPath;
        private Node2D _shroomVisualReference;
        [Export]
        private NodePath _physicalCollisionPath;
        private CollisionShape2D _physicalCollisionReference;
        [Export]
        private NodePath _interactionCollisionPath;
        private CollisionShape2D _interactionCollisionReference;
        [Export]
        private NodePath _movementPath;
        private BasicMovement _movementReference;

        public override void _Ready()
        {
            SetNodeReferences();
            _movementReference.BodyToMove = this;
        }

        private void SetNodeReferences()
        {
            _shroomVisualReference = GetNode<Node2D>(_shroomVisualPath);
            _physicalCollisionReference = GetNode<CollisionShape2D>(_physicalCollisionPath);
            _interactionCollisionReference = GetNode<CollisionShape2D>(_interactionCollisionPath);
            _movementReference = GetNode<BasicMovement>(_movementPath);
        }

        protected void CollectShroom()
        {
            _shroomVisualReference.Visible = false;
            _interactionCollisionReference.SetDeferred("disabled", true);
            _movementReference.ShouldMove = false;
        }

        public virtual void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "powerup_rise")
            {
                _physicalCollisionReference.SetDeferred("disabled", false);
                _interactionCollisionReference.SetDeferred("disabled", false);
                _movementReference.ShouldMove = true;
            }
        }

    }
}