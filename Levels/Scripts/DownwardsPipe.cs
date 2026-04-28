using Godot;

namespace Game.Levels
{
    [Tool]
    public class DownwardsPipe : Pipe
    {
        public override bool CanEnterPipe()
        {
            foreach (RayCast2D ray in PipeRayCasts)
            {
                if (!ray.IsColliding())
                {
                    return false;
                }
            }
            return true;
        }
    }
}