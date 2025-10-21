using Game.Player;
using Godot;
using Util.ExtensionMethods;

namespace Game.Blocks
{

    public class Brick : Block
    {
        [Export]
        private NodePath _brickHitSoundPath;
        private AudioStreamPlayer _brickHitSoundReference;
        [Export]
        private NodePath _brickBreakSoundPath;
        private AudioStreamPlayer _brickBreakSoundReference;
        [Export]
        private NodePath _physicalHitBoxPath;
        private CollisionShape2D _physicalHitBoxReference;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _brickHitSoundReference = GetNode<AudioStreamPlayer>(_brickHitSoundPath);
            _physicalHitBoxReference = GetNode<CollisionShape2D>(_physicalHitBoxPath);
            _brickBreakSoundReference = GetNode<AudioStreamPlayer>(_brickBreakSoundPath);
        }


        public override void OnBlockHitByPlayer(Size playerSize)
        {
            switch (ItemInBlock)
            {
                case Item.None:
                    AttemptBrickBreak(playerSize);
                    break;
            }
        }

        private void AttemptBrickBreak(Size playerSize)
        {
            if (playerSize == Size.Small)
            {
                BounceAnimationReference.Play("bounce");
                _brickHitSoundReference.Play();
            }
            else
            {
                BreakBrick();
            }
        }

        private void BreakBrick()
        {
            InteractionHitBoxReference.SetDeferred("disabled", true);
            _physicalHitBoxReference.SetDeferred("disabled", true);
            BlockVisualReference.Visible = false;
            _brickBreakSoundReference.Play();
            PackedScene particleScene = GD.Load<PackedScene>("res://Blocks/Scenes/BrickParticle.tscn");
            OneShotParticle brickParticle = particleScene.Instance<OneShotParticle>();
            brickParticle.Connect("ParticleFinished", this, nameof(OnParticleFinished));
            AddChild(brickParticle);
        }

        public void OnParticleFinished()
        {
            this.SafeQueueFree();
        }

    }
}