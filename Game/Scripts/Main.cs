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
        private NodePath _currentLevelPath;
        private Level _currentLevelReference;

        public override void _Ready()
        {
            SetNodeReferences();
            _controller.CharacterToControl = _player;
            _currentLevelReference.Start();
        }

        private void SetNodeReferences()
        {
            _controller = GetNode<PlayerController>(_controllerPath);
            _player = GetNode<Vito>(_playerPath);
            _currentLevelReference = GetNode<Level>(_currentLevelPath);
        }
    }
}