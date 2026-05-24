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

        public override void DisableProjectile()
        {
            FireballSoundReference.Stop();
            Disable();
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
            MovementReference.StopVelocity();
            MovementReference.CanMove = false;
            TopWallDetectorReference.Enabled = false;
            BottomWallDetectorReference.Enabled = false;
            HitboxReference.SetDeferred("disabled", true);
            PhysicalHitboxReference.SetDeferred("disabled", true);
            VisualReference.Visible = false;
            VisualReference.Stop();
            VisualReference.Frame = 0;
            Enabled = false;
        }
    }
}
