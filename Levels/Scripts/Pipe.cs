using Godot;
using Godot.Collections;
using Util.ExtensionMethods;
using Game.Player;

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
        [Export]
        private NodePath[] _rayCastPaths;
        private Array<RayCast2D> _pipeRayCasts = new Array<RayCast2D>();
        protected Array<RayCast2D> PipeRayCasts
        {
            get { return _pipeRayCasts; }
        }
        [Export]
        private bool _playExitAnimation;

        public override void _Ready()
        {
            if (Engine.EditorHint)
            {
                Node markerContainer = new Node();
                AddChild(markerContainer);
                PackedScene levelMarkerScene = GD.Load<PackedScene>("res://Levels/Scenes/LevelMarker.tscn");
                _pipeExitPointReference = levelMarkerScene.Instance<LevelMarker>();
                _pipeExitPointReference.Position = _pipeExitPoint;
                markerContainer.AddChild(_pipeExitPointReference);
                _pipeExitPointReference.MarkerIconReference.Texture = GD.Load<Texture>("res://Player/Art/VitoIdle.png");
                _cameraExitPointReference = levelMarkerScene.Instance<LevelMarker>();
                _cameraExitPointReference.Position = _cameraExitPoint;
                markerContainer.AddChild(_cameraExitPointReference);
                _cameraExitPointReference.MarkerIconReference.Texture = GD.Load<Texture>("res://Levels/Art/CameraIcon.png");
                return;
            }
            SetNodeConnections();
            SetPipeRayCasts();
        }

        private void SetNodeConnections()
        {
            _pipeSoundPlayerReference = GetNode<AudioStreamPlayer>(_pipeSoundPlayerPath);
        }

        private void SetPipeRayCasts()
        {
            foreach (NodePath path in _rayCastPaths)
            {
                _pipeRayCasts.Add(GetNode<RayCast2D>(path));
            }
        }

        public void PlayPipeSound()
        {
            PipeSoundPlayerReference.Play();
        }

        public void OnBodyEntered(Node2D body)
        {
            if (body is Vito vito)
            {
                vito.OverlappedPipe = this;
            }
        }

        public void OnBodyExited(Node2D body)
        {
            if (body is Vito vito)
            {
                vito.OverlappedPipe = null;
            }
        }

        public void OnPipeSoundFinished()
        {
            LevelEventBus.Instance.EmitSignal("PipeEntranceFinished", _playExitAnimation);
        }

        public abstract bool CanEnterPipe();
    }
}