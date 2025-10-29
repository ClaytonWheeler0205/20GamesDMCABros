using Godot;
using Util.ExtensionMethods;

namespace Game.Items
{
    public class BlockCoin : Node2D
    {

        [Export]
        private NodePath _coinSpritePath;
        private AnimatedSprite _coinSpriteReference;
        [Export]
        private NodePath _pointsSpritePath;
        private Node2D _pointsSpriteReference;
        [Export]
        private NodePath _coinAnimationPlayerPath;
        private AnimationPlayer _coinAnimationPlayerReference;

        public override void _Ready()
        {
            SetNodeReferences();
            _coinSpriteReference.Play();
            //TODO: Give the player 200 points
        }

        private void SetNodeReferences()
        {
            _coinSpriteReference = GetNode<AnimatedSprite>(_coinSpritePath);
            _pointsSpriteReference = GetNode<Node2D>(_pointsSpritePath);
            _coinAnimationPlayerReference = GetNode<AnimationPlayer>(_coinAnimationPlayerPath);
        }

        public void OnCoinAnimationFinished(string anim_name)
        {
            if (anim_name == "coin_bounce")
            {
                _coinSpriteReference.Stop();
                _coinSpriteReference.Visible = false;
                _pointsSpriteReference.Visible = true;
                _coinAnimationPlayerReference.Play("point_float");
            }
            else
            {
                this.SafeQueueFree();
            }
        }
    }
}