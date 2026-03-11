using Godot;
using Util.ExtensionMethods;

namespace Game.Levels
{

    public abstract class Pipe : Area2D
    {

        private LevelMarker _pipeExitPointReference;
        private Vector2 _pipeExitPoint = Vector2.Zero;
        [Export]
        public Vector2 PipeExitPoint
        {
            get { return _pipeExitPoint; }
            set
            {
                _pipeExitPoint = value;
                if (_pipeExitPointReference.IsValid())
                {
                    _pipeExitPointReference.Position = _pipeExitPoint;
                }
            }
        }
        private LevelMarker _cameraExitPointReference;
        private Vector2 _cameraExitPoint = Vector2.Zero;
        [Export]
        public Vector2 CameraExitPoint
        {
            get { return _cameraExitPoint; }
            set
            {
                _cameraExitPoint = value;
                if (_cameraExitPointReference.IsValid())
                {
                    _cameraExitPointReference.Position = _cameraExitPoint;
                }
            }
        }
        [Export]
        private NodePath _pipeSoundPlayerPath;
        private AudioStreamPlayer _pipeSoundPlayerReference;
        protected AudioStreamPlayer PipeSoundPlayerReference
        {
            get { return _pipeSoundPlayerReference; }
        }

        public override void _Ready()
        {
            if (Engine.EditorHint)
            {
                PackedScene levelMarkerScene = GD.Load<PackedScene>("res://Levels/Scenes/LevelMarker.tscn");
                _pipeExitPointReference = levelMarkerScene.Instance<LevelMarker>();
                _pipeExitPointReference.Position = _pipeExitPoint;
                AddChild(_pipeExitPointReference);
                _pipeExitPointReference.MarkerIconReference.Texture = GD.Load<Texture>("res://Player/Art/VitoIdle.png");
                _cameraExitPointReference = levelMarkerScene.Instance<LevelMarker>();
                _cameraExitPointReference.Position = _cameraExitPoint;
                AddChild(_cameraExitPointReference);
                _cameraExitPointReference.MarkerIconReference.Texture = GD.Load<Texture>("res://Levels/Art/CameraIcon.png");
                return;
            }
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            _pipeSoundPlayerReference = GetNode<AudioStreamPlayer>(_pipeSoundPlayerPath);
        }

        public abstract bool CanEnterPipe();
        public abstract void PlayPipeSound();
        public abstract void OnBodyEntered(Node2D body);
        public abstract void OnBodyExited(Node2D body);
        public abstract void OnPipeSoundFinished();
    }
}