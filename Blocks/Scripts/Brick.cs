using Game.Buses;
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
        private const int BRICK_POINT_VALUE = 50;

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

        public void OnBlockHitBySmallPlayer()
        {
            BounceBrick();
        }

        public void OnBlockHitByBigPlayer()
        {
            BreakBrick();
        }

        private void BounceBrick()
        {
            BounceAnimationReference.Play("bounce");
            _brickHitSoundReference.Play();
            BlockDamageReference.SetDeferred("disabled", false);
        }

        private async void BreakBrick()
        {
            InteractionHitBoxReference.SetDeferred("disabled", true);
            BlockVisualReference.Visible = false;
            _brickBreakSoundReference.Play();
            PackedScene particleScene = GD.Load<PackedScene>("res://Blocks/Scenes/BrickParticle.tscn");
            OneShotParticle brickParticle = particleScene.Instance<OneShotParticle>();
            AddChild(brickParticle);
            PointsEventBus.Instance.EmitSignal("PointsGained", BRICK_POINT_VALUE);
            BlockDamageReference.SetDeferred("disabled", false);
            await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
            _physicalHitBoxReference.SetDeferred("disabled", true);
            BlockDamageReference.SetDeferred("disabled", true);
        }

        public override void EnableBlock()
        {
            InteractionHitBoxReference.SetDeferred("disabled", false);
            BlockVisualReference.Show();
            _physicalHitBoxReference.SetDeferred("disabled", false);
        }

        public override void OnAnimationFinished(string anim_name)
        {
            if (anim_name != "bounce")
            {
                return;
            }
            BlockDamageReference.SetDeferred("disabled", true);
        }
    }
}