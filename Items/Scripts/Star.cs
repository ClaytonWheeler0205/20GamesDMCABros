using Godot;
using Util.ExtensionMethods;

namespace Game.Items
{

    public class Star : KinematicBody2D
    {
        [Export]
        private NodePath _starVisualPath;
        private AnimatedSprite _starVisualReference;
        [Export]
        private NodePath _scoreTextVisualPath;
        private Node2D _scoreTextVisualReference;
        [Export]
        private NodePath _physicalCollisionPath;
        private CollisionShape2D _physicalCollisionReference;
        [Export]
        private NodePath _interactionCollisionPath;
        private CollisionShape2D _interactionCollisionReference;
        [Export]
        private NodePath _starAnimationPath;
        private AnimationPlayer _starAnimationReference;
        [Export]
        private NodePath _movementPath;
        private BouncyMovement _movementReference;
        [Export]
        private NodePath _powerupGetSoundPath;
        private AudioStreamPlayer _powerupGetSoundReference;

        public override void _Ready()
        {
            SetNodeReferences();
            _movementReference.BodyToMove = this;
            _starVisualReference.Play();
        }

        private void SetNodeReferences()
        {
            _starVisualReference = GetNode<AnimatedSprite>(_starVisualPath);
            _scoreTextVisualReference = GetNode<Node2D>(_scoreTextVisualPath);
            _physicalCollisionReference = GetNode<CollisionShape2D>(_physicalCollisionPath);
            _interactionCollisionReference = GetNode<CollisionShape2D>(_interactionCollisionPath);
            _starAnimationReference = GetNode<AnimationPlayer>(_starAnimationPath);
            _movementReference = GetNode<BouncyMovement>(_movementPath);
            _powerupGetSoundReference = GetNode<AudioStreamPlayer>(_powerupGetSoundPath);
        }

        public void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "star_rise")
            {
                _physicalCollisionReference.SetDeferred("disabled", false);
                _interactionCollisionReference.SetDeferred("disabled", false);
                _movementReference.CanMove = true;
            }
            else if (anim_name == "score_float")
            {
                this.SafeQueueFree();
            }
        }

        public void OnBodyEntered(Node body)
        {
            if (body.IsInGroup("player"))
            {
                _starVisualReference.Visible = false;
                _scoreTextVisualReference.Visible = true;
                _movementReference.CanMove = false;
                _interactionCollisionReference.SetDeferred("disabled", true);
                _powerupGetSoundReference.Play();
                _starAnimationReference.Play("score_float");
                //TODO: give the player 100 points
                //TODO: give the player invincibility
            }
        }
    }
}