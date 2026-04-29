using Godot;

namespace Game.Levels
{
    [Tool]
    public class SidewaysPipe : Pipe
    {
        public override void _Ready()
        {
            base._Ready();
        }

        public override bool CanEnterPipe()
        {
            foreach (RayCast2D ray in PipeRayCasts)
            {
                if (ray.IsColliding())
                {
                    return true;
                }
            }
            return false;
        }
    }
}