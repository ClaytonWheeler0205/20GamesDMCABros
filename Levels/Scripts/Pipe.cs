using Godot;
using Util.ExtensionMethods;

namespace Game.Levels
{

    public abstract class Pipe : Area2D
    {

        private Position2D _pipeExitPointReference;
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
                _pipeExitPointReference = new Position2D();
                _pipeExitPointReference.Position = _pipeExitPoint;
                AddChild(_pipeExitPointReference);
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