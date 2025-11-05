using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public interface PlayerAnimator
    {
        Vito PlayerToAnimate { get; set; }
        MovementComponent PlayerMovement { get; set; }
        void ToggleAnimation();
    }
}