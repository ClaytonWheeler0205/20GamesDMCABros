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

        public override void _Ready()
        {
            SetNodeReferences();
            _controller.CharacterToControl = _player;
        }

        private void SetNodeReferences()
        {
            _controller = GetNode<PlayerController>(_controllerPath);
            _player = GetNode<Vito>(_playerPath);
        }
    }
}