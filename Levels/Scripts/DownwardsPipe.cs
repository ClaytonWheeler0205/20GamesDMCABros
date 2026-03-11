using Game.Player;
using Godot;
using Godot.Collections;

namespace Game.Levels
{

    [Tool]
    public class DownwardsPipe : Pipe
    {
        [Export]
        private NodePath[] _rayCastPaths;
        private Array<RayCast2D> _pipeRayCasts = new Array<RayCast2D>();

        public override void _Ready()
        {
            base._Ready();
            if (Engine.EditorHint)
            {
                return;
            }
            SetPipeRayCasts();
        }

        private void SetPipeRayCasts()
        {
            foreach (NodePath path in _rayCastPaths)
            {
                _pipeRayCasts.Add(GetNode<RayCast2D>(path));
            }
        }

        public override bool CanEnterPipe()
        {
            foreach (RayCast2D ray in _pipeRayCasts)
            {
                if (!ray.IsColliding())
                {
                    return false;
                }
            }
            return true;
        }

        public override void PlayPipeSound()
        {
            PipeSoundPlayerReference.Play();
        }

        public override void OnBodyEntered(Node2D body)
        {
            if (body is Vito vito)
            {
                vito.OverlappedPipe = this;
            }
        }

        public override void OnBodyExited(Node2D body)
        {
            if (body is Vito vito)
            {
                vito.OverlappedPipe = null;
            }
        }

        public override void OnPipeSoundFinished()
        {
            LevelEventBus.Instance.EmitSignal("PipeEntranceFinished");
        }
    }
}