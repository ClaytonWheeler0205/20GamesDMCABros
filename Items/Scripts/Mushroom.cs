using Game.Buses;
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
        private const int MUSHROOM_POINT_VALUE = 1000;

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

        public override void OnBodyEntered(Node body)
        {
            if (body is Vito vito)
            {
                CollectShroom();
                _pointTextReference.Visible = true;
                _shroomAnimationReference.Play("score_float");
                PointsEventBus.Instance.EmitSignal("PointsGained", MUSHROOM_POINT_VALUE);
                vito.GrowBig();
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