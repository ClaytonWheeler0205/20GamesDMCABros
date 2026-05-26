using Game.Buses;
using Game.Levels;
using Game.Player;
using Game.UI;
using Godot;

namespace Game
{

    public class Main : Node
    {
        [Export]
        private NodePath _controllerPath;
        private PlayerController _controller;
        [Export]
        private NodePath _playerPath;
        private Vito _player;
        [Export]
        private NodePath _cameraPath;
        private Camera _camera;
        [Export]
        private NodePath _currentLevelPath;
        private Level _currentLevel;
        private bool _newLevel = true;
        [Export]
        private NodePath _levelStartScreenPath;
        private CanvasItem _levelStartScreen;
        [Export]
        private NodePath _gameOverScreenPath;
        private CanvasItem _gameOverScreen;
        [Export]
        private NodePath _timeUpScreenPath;
        private CanvasItem _timeUpScreen;
        [Export]
        private NodePath _scoreboardPath;
        private Scoreboard _scoreboardReference;
        private bool _gameOver = false;
        private bool _timeUp = false;

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            _controller.CharacterToControl = _player;
            GetTree().Paused = true;
            StartGame();
        }

        private void SetNodeReferences()
        {
            _controller = GetNode<PlayerController>(_controllerPath);
            _player = GetNode<Vito>(_playerPath);
            _camera = GetNode<Camera>(_cameraPath);
            _currentLevel = GetNode<Level>(_currentLevelPath);
            _levelStartScreen = GetNode<CanvasItem>(_levelStartScreenPath);
            _gameOverScreen = GetNode<CanvasItem>(_gameOverScreenPath);
            _timeUpScreen = GetNode<CanvasItem>(_timeUpScreenPath);
            _scoreboardReference = GetNode<Scoreboard>(_scoreboardPath);
        }

        private void SetNodeConnections()
        {
            LivesEventBus.Instance.Connect("LifeLostUpdated", this, nameof(OnLifeLostUpdated));
            LivesEventBus.Instance.Connect("GameOver", this, nameof(OnGameOver));
            TimerEventBus.Instance.Connect("TimeUp", this, nameof(OnTimeUp));
        }

        private async void StartGame()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _levelStartScreen.Visible = false;
            _scoreboardReference.TimeUIReference.ResetTimer();
            _scoreboardReference.TimeUIReference.ShowTimer();
            GetTree().Paused = false;
            _player.Damageable = true;
            _currentLevel.Start(_newLevel);
            _newLevel = false;
            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            _player.ResetPlayer();
        }

        public async void OnLifeLostUpdated()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _scoreboardReference.TimeUIReference.HideTimer();
            if (_gameOver)
            {
                _gameOverScreen.Visible = true;
                JinglePlayer.Instance.PlayJingle(JingleType.GameOver);
                return;
            }
            if (_timeUp)
            {
                _timeUpScreen.Visible = true;
                await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
                _timeUpScreen.Visible = false;
                _currentLevel.ResetPlayerSpawnPoint();
                _scoreboardReference.TimeUIReference.StartTimer();
                _timeUp = false;
            }
            ResetGame();
        }

        private void ResetGame()
        {
            _camera.GlobalPosition = _currentLevel.CameraPointPosition;
            _camera.ApplyCameraPosition();
            _player.GlobalPosition = _currentLevel.GetPlayerSpawnPoint();
            GetTree().CallGroup("block_item", "queue_free");
            GetTree().CallGroup("audio_source", "stop");
            GetTree().CallGroup("point_text", "queue_free");
            _currentLevel.ResetLevel();
            _levelStartScreen.Visible = true;
            StartGame();
        }

        public void OnGameOver()
        {
            _gameOver = true;
        }

        public void OnTimeUp()
        {
            _timeUp = true;
        }
    }
}
