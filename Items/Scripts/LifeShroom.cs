using Game.Buses;
using Godot;
using Util.ExtensionMethods;


namespace Game.Items
{

    public class LifeShroom : Shroom
    {
        [Export]
        private NodePath _lifeTextPath;
        private Node2D _lifeTextReference;
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
            _lifeTextReference = GetNode<Node2D>(_lifeTextPath);
            _shroomAnimationReference = GetNode<AnimationPlayer>(_shroomAnimationPath);
        }

        public override void OnBodyEntered(Node body)
        {
            if (body.IsInGroup("player"))
            {
                CollectShroom();
                _lifeTextReference.Visible = true;
                _shroomAnimationReference.Play("life_float");
                LivesEventBus.Instance.EmitSignal("GainLife");
            }
        }

        public override void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "life_float")
            {
                this.SafeQueueFree();
                return;
            }
            base.OnAnimationFinished(anim_name);
        }
    }
}