using Godot;
using Util.ExtensionMethods;

namespace Game.Projectiles
{

    public class FireballImpl : Fireball
    {

        public override async void Enable()
        {
            await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            MovementReference.ResetVelocity();
            MovementReference.CanMove = true;
            TopWallDetectorReference.Enabled = true;
            BottomWallDetectorReference.Enabled = true;
            HitboxReference.SetDeferred("disabled", false);
            PhysicalHitboxReference.SetDeferred("disabled", false);
            VisualReference.Visible = true;
            VisualReference.Play("rolling");
            FireballSoundReference.Play();
            Enabled = true;
        }

        public void OnAnimationFinished()
        {
            if (VisualReference.Animation != "explosion")
            {
                return;
            }
            Disable();
        }

        public void OnScreenExited()
        {
            Disable();
        }

        private void Disable()
        {
            VisualReference.Visible = false;
            Enabled = false;
        }
    }
}