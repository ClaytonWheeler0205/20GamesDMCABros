using Game.Player;
using Godot;
using Util.ExtensionMethods;

namespace Game.Items
{
    public class Mushroom : Shroom
    {
        [Export]
        private NodePath _pointTextPath;
        private Node2D _pointTextReference;
        [Export]
        private NodePath _shroomAnimationPath;
        private AnimationPlayer _shroomAnimationReference;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
        }
        
        private void SetNodeReferences()
        {
            _pointTextReference = GetNode<Node2D>(_pointTextPath);
            _shroomAnimationReference = GetNode<AnimationPlayer>(_shroomAnimationPath);
        }

        public void OnBodyEntered(Node body)
        {
            if (body.IsInGroup("player"))
            {
                GlobalPlayerData.PlayerSize = Size.Big;
                CollectShroom();
                _pointTextReference.Visible = true;
                _shroomAnimationReference.Play("score_float");
                //TODO: update the score
            }
        }

        public override void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "score_float")
            {
                this.SafeQueueFree();
                return;
            }
            base.OnAnimationFinished(anim_name);
        }

    }
}