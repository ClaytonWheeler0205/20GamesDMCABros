using Godot;
using Util.ExtensionMethods;

namespace Game.Items
{

    public class FireFlower : Node2D
    {
        [Export]
        private NodePath _flowerVisualPath;
        private AnimatedSprite _flowerVisualReference;
        [Export]
        private NodePath _pointTextPath;
        private Node2D _pointTextReference;
        [Export]
        private NodePath _collisionPath;
        private CollisionShape2D _collisionReference;
        [Export]
        private NodePath _flowerAnimationPath;
        private AnimationPlayer _flowerAnimationReference;
        [Export]
        private NodePath _powerupGetSoundPath;
        private AudioStreamPlayer _powerupGetSoundReference;

        public override void _Ready()
        {
            SetNodeReferences();
            _flowerVisualReference.Play();
        }

        private void SetNodeReferences()
        {
            _flowerVisualReference = GetNode<AnimatedSprite>(_flowerVisualPath);
            _pointTextReference = GetNode<Node2D>(_pointTextPath);
            _collisionReference = GetNode<CollisionShape2D>(_collisionPath);
            _flowerAnimationReference = GetNode<AnimationPlayer>(_flowerAnimationPath);
            _powerupGetSoundReference = GetNode<AudioStreamPlayer>(_powerupGetSoundPath);
        }

        public void OnBodyEntered(Node body)
        {
            if (body.IsInGroup("player"))
            {
                CollectFlower();
            }
        }

        private void CollectFlower()
        {
            _flowerVisualReference.Stop();
            _flowerVisualReference.Visible = false;
            _pointTextReference.Visible = true;
            _collisionReference.SetDeferred("disabled", true);
            _powerupGetSoundReference.Play();
            _flowerAnimationReference.Play("score_float");
        }
        
        public void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "powerup_rise")
            {
                _collisionReference.SetDeferred("disabled", false);
            }
            else if (anim_name == "score_float")
            {
                this.SafeQueueFree();
            }
        }
    }
}
