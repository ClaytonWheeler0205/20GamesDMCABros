using Game.Buses;
using Game.Levels;
using Game.Player;
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
        [Export]
        private NodePath _levelStartScreenPath;
        private CanvasItem _levelStartScreen;
        [Export]
        private NodePath _gameOverScreenPath;
        private CanvasItem _gameOverScreen;
        private bool _gameOver = false;

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
        }

        private void SetNodeConnections()
        {
            LivesEventBus.Instance.Connect("LifeLostUpdated", this, nameof(OnLifeLostUpdated));
            LivesEventBus.Instance.Connect("GameOver", this, nameof(OnGameOver));
        }

        private async void StartGame()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            _levelStartScreen.Visible = false;
            GetTree().Paused = false;
            _player.Damageable = true;
            _currentLevel.Start();
            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            _player.ResetPlayer();
        }

        public async void OnLifeLostUpdated()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
            if (_gameOver)
            {
                _gameOverScreen.Visible = true;
                JinglePlayer.Instance.PlayJingle(JingleType.GameOver);
                return;
            }
            ResetGame();
        }

        private void ResetGame()
        {
            _camera.GlobalPosition = new Vector2(_currentLevel.GetPlayerSpawnPoint().x, _camera.GlobalPosition.y);
            _camera.ApplyCameraPosition();
            _player.GlobalPosition = _currentLevel.GetPlayerSpawnPoint();
            _currentLevel.ResetLevel();
            _levelStartScreen.Visible = true;
            StartGame();
        }

        public void OnGameOver()
        {
            _gameOver = true;
        }
    }
}