using Godot;

namespace Game.Debug
{

    public class VitoDebug : Control
    {
        [Export]
        private NodePath _directionDisplayPath;
        private Label _directionDisplayReference;
        [Export]
        private NodePath _groundDisplayPath;
        private Label _groundDisplayReference;

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _directionDisplayReference = GetNode<Label>(_directionDisplayPath);
            _groundDisplayReference = GetNode<Label>(_groundDisplayPath);
        }

        public void DisplayDirection(float direction)
        {
            _directionDisplayReference.Text = $"Directon: {direction}";
        }

        public void DisplayGround(bool isOnGround)
        {
            _groundDisplayReference.Text = $"On Ground: {isOnGround}";
        }
    }
}