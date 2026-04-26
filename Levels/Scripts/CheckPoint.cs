using Godot;
using Util.ExtensionMethods;

namespace Game.Levels
{

	[Tool]
	public class CheckPoint : Area2D
	{
		[Signal]
		public delegate void PlayerReachedCheckpoint(Vector2 checkpointPosition, Vector2 cameraPosition);

		private LevelMarker _playerRespawnPointReference;
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
		private LevelMarker _cameraRespawnPointReference;
		private Vector2 _cameraRespawnPoint = Vector2.Zero;
		[Export]
		public Vector2 CameraRespawnPoint
		{
			get { return _cameraRespawnPoint; }
			set
			{
				_cameraRespawnPoint = value;
				if (_cameraRespawnPointReference.IsValid())
				{
					_cameraRespawnPointReference.Position = _cameraRespawnPoint;
				}
			}
		}

		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				return;
			}
			PackedScene levelMarkerScene = GD.Load<PackedScene>("res://Levels/Scenes/LevelMarker.tscn");
			_playerRespawnPointReference = levelMarkerScene.Instance<LevelMarker>();
			_playerRespawnPointReference.Position = _playerRespawnPoint;
			AddChild(_playerRespawnPointReference);
			_playerRespawnPointReference.MarkerIconReference.Texture = GD.Load<Texture>("res://Levels/Art/CheckpointIcon.png");
			_cameraRespawnPointReference = levelMarkerScene.Instance<LevelMarker>();
			_cameraRespawnPointReference.Position = _cameraRespawnPoint;
			AddChild(_cameraRespawnPointReference);
			_cameraRespawnPointReference.MarkerIconReference.Texture = GD.Load<Texture>("res://Levels/Art/CameraIcon.png");
		}


		public void OnBodyEntered(Node body)
		{
			if (!body.IsInGroup("player"))
			{
				return;
			}
			EmitSignal("PlayerReachedCheckpoint", _playerRespawnPoint + GlobalPosition, _cameraRespawnPoint + GlobalPosition);
		}
	}
}
