using Godot;

namespace Game.UI
{

    public class TitleScreen : Node
    {
        [Export]
        private NodePath _highScoreTextPath;
        private Label _highScoreTextReference;

        private ConfigFile _config = new ConfigFile();

        public override void _Ready()
        {
            _highScoreTextReference = GetNode<Label>(_highScoreTextPath);
            DisplayHighScore();
            GetTree().Paused = false;
        }

        private void DisplayHighScore()
        {
            LoadHighScore();
        }

        private void LoadHighScore()
        {
            Error err = _config.Load("user://dmcabros_highscore.cfg");

            if (err != Error.Ok)
                return;

            int highScore = (int)_config.GetValue("DCMABrosPlayerScore", "dmcabros_high_score");
            UpdateHighScore(highScore);
        }

        private void UpdateHighScore(int score)
        {
            if (score < 10)
            {
                _highScoreTextReference.Text = $"TOP - 00000{score}";
            }
            else if (score < 100)
            {
                _highScoreTextReference.Text = $"TOP - 0000{score}";
            }
            else if (score < 1000)
            {
                _highScoreTextReference.Text = $"TOP - 000{score}";
            }
            else if (score < 10000)
            {
                _highScoreTextReference.Text = $"TOP - 00{score}";
            }
            else if (score < 100000)
            {
                _highScoreTextReference.Text = $"TOP - 0{score}";
            }
            else
            {
                _highScoreTextReference.Text = $"TOP - {score}";
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("start"))
                GetTree().ChangeScene("res://Game/Scenes/Game.tscn");
        }
    }
}
