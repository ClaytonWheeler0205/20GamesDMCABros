using Godot;
using Util.ExtensionMethods;

namespace Game.Levels
{

    [Tool]
    public class CheckPoint : Area2D
    {
        [Signal]
        public delegate void PlayerReachedCheckpoint(Vector2 checkpointPosition);

        private Position2D _playerRespawnPointReference;
        private Vector2 _playerRespawnPoint = Vector2.Zero;
        [Export]
        public Vector2 PlayerRespawnPoint
        {
            get { return _playerRespawnPoint; }
            set
            {
                _playerRespawnPoint = value;
                if (_playerRespawnPointReference.IsValid())
                {
                    _playerRespawnPointReference.Position = _playerRespawnPoint;
                }
            }
        }

        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                return;
            }
            _playerRespawnPointReference = new Position2D();
            _playerRespawnPointReference.Position = _playerRespawnPoint;
            AddChild(_playerRespawnPointReference);
        }


        public void OnBodyEntered(Node body)
        {
            if (!body.IsInGroup("player"))
            {
                return;
            }
            EmitSignal("PlayerReachedCheckpoint", _playerRespawnPoint + GlobalPosition);
        }
    }
}