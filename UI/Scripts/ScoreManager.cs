using Game.Buses;
using Godot;

namespace Game.UI
{

    public class ScoreManager : Node
    {
        [Export]
        private NodePath _scoreTextPath;
        private Label _scoreTextReference;
        private int _points;

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _scoreTextReference = GetNode<Label>(_scoreTextPath);
        }

        private void SetNodeConnections()
        {
            PointsEventBus.Instance.Connect("PointsGained", this, nameof(OnPointsGained));
        }

        public void OnPointsGained(int pointValue)
        {
            _points += pointValue;
            UpdateScoreText();
        }
        private void UpdateScoreText()
        {
            if (_points < 100)
            {
                _scoreTextReference.Text = $"0000{_points}";
            }
            else if (_points < 1000)
            {
                _scoreTextReference.Text = $"000{_points}";
            }
            else if (_points < 10000)
            {
                _scoreTextReference.Text = $"00{_points}";
            }
            else if (_points < 100000)
            {
                _scoreTextReference.Text = $"0{_points}";
            }
            else
            {
                _scoreTextReference.Text = $"{_points}";
            }
        }
    }
}