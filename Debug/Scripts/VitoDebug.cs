using Godot;

namespace Game.Debug
{

    public class VitoDebug : Control
    {
        [Export]
        private NodePath _directionDisplayPath;
        private Label _directionDisplayReference;

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _directionDisplayReference = GetNode<Label>(_directionDisplayPath);
        }

        public void DisplayDirection(float direction)
        {
            _directionDisplayReference.Text = $"Directon: {direction}";
        }
    }
}