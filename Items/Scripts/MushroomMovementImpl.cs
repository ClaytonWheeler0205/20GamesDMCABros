using Godot;

namespace Game.Items
{

    public class MushroomMovementImpl : MushroomMovement
    {
        public override void Bounce()
        {
            Velocity = new Vector2(Velocity.x, BounceForce);
        }
    }
}