using Godot;

namespace Game.Levels
{
    public class SidewaysPipe : Pipe
    {
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