using Game.Player;
using Godot;

namespace Game.Blocks
{

    public class BlockHitbox : Area2D
    {
        [Signal]
        public delegate void BlockHitByPlayer(Size playerSize);

        public void OnAreaEntered(Area2D area)
        {
            if (area is JumpHitbox jumpHitbox && !jumpHitbox.HasHitBlock)
            {
                jumpHitbox.HasHitBlock = true;
                HandlePlayerCollisions(jumpHitbox);
            }
        }

        private void HandlePlayerCollisions(JumpHitbox jumpData)
        {
            if (jumpData.VerticalVelocity <= 0.0f)
            {
                EmitSignal("BlockHitByPlayer", jumpData.PlayerSize);
            }
        }
    }
}