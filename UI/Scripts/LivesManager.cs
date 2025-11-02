using Godot;
using Game.Buses;

namespace Game.UI
{

    public class LivesManager : Control
    {
        [Export]
        private NodePath _livesTextPath;
        private Label _livesTextReference;
        private int _lives = 3;

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _livesTextReference = GetNode<Label>(_livesTextPath);
        }

        private void SetNodeConnections()
        {
            LivesEventBus.Instance.Connect("LoseLife", this, nameof(OnLifeLost));
            LivesEventBus.Instance.Connect("GainLife", this, nameof(OnLifeGained));
        }

        public void OnLifeLost()
        {
            _lives--;
            if (_lives == 0)
            {
                LivesEventBus.Instance.EmitSignal("GameOver");
                return;
            }
            UpdateLifeText();
        }

        public void OnLifeGained()
        {
            _lives++;
            UpdateLifeText();
        }
        
        private void UpdateLifeText()
        {
            _livesTextReference.Text = $"  x   {_lives}";
        }
    }
}