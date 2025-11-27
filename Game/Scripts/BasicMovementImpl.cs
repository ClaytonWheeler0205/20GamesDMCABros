using Game;
using Godot;


public class BasicMovementImpl : BasicMovement
{
    public override void FlipDirection()
    {
        switch (MovementDirection)
        {
            case Direction.Left:
                MovementDirection = Direction.Right;
                break;
            case Direction.Right:
                MovementDirection = Direction.Left;
                break;
        }
        WallDetectorReference.CastTo = -1 * WallDetectorReference.CastTo;
        EmitSignal("DirectionFlipped");
    }

}
