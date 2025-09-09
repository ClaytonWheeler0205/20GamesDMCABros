using Godot;

namespace Game.Player
{

    public class PlayerControllerImpl : PlayerController
    {
        public override void _Process(float delta)
        {
            CharacterToControl.SetMovementDirection(Input.GetAxis("move_left", "move_right"));
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("jump"))
            {
                CharacterToControl.Jump();
            }
            else if (@event.IsActionReleased("jump"))
            {
                CharacterToControl.ReleaseJump();
            }
            if (@event.IsActionPressed("run"))
            {
                CharacterToControl.StartRunning();
            }
            else if (@event.IsActionReleased("run"))
            {
                CharacterToControl.StopRunning();
            }
        }

    }
}