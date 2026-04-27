using Godot;
using Godot.Collections;
using Game.Player;

namespace Game.Levels
{

    public class SidewaysPipe : Pipe
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
                if (ray.IsColliding())
                {
                    return true;
                }
            }
            return false;
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

        public override void PlayPipeSound()
        {
            PipeSoundPlayerReference.Play();
        }
    }
}