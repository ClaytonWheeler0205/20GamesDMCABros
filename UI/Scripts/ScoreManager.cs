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
        private int _highScore;

        private ConfigFile _config = new ConfigFile();

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            LoadHighScore();
        }

        private void SetNodeReferences()
        {
            _scoreTextReference = GetNode<Label>(_scoreTextPath);
        }

        private void SetNodeConnections()
        {
            PointsEventBus.Instance.Connect("PointsGained", this, nameof(OnPointsGained));
        }

        private void LoadHighScore()
        {
            Error err = _config.Load("user://dmcabros_highscore.cfg");

            if (err != Error.Ok)
                return;

            _highScore = (int)_config.GetValue("DCMABrosPlayerScore", "dmcabros_high_score");
        }

        public void OnPointsGained(int pointValue)
        {
            _points += pointValue;
            UpdateScoreText();
            if (_points > _highScore)
            {
                _highScore = _points;
                SaveHighScore();
            }
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

        private void SaveHighScore()
        {
            _config.SetValue("DCMABrosPlayerScore", "dmcabros_high_score", _highScore);
            _config.Save("user://dmcabros_highscore.cfg");
        }
    }
}
