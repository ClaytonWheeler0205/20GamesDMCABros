using Godot;
using Util.ExtensionMethods;

namespace Game.Projectiles
{

    public class FireballImpl : Fireball
    {

        public override void Enable()
        {
            MovementReference.ResetVelocity();
            MovementReference.CanMove = true;
            TopWallDetectorReference.Enabled = true;
            BottomWallDetectorReference.Enabled = true;
            HitboxReference.SetDeferred("disabled", false);
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
            EmitSignal("FireballDestroyed");
            Disable();
        }

        public void OnScreenExited()
        {
            EmitSignal("FireballDestroyed");
            Disable();
        }

        public void OnBodyEntered(Node body)
        {
            if (!body.IsInGroup("enemy"))
            {
                return;
            }
            DestroyFireball();
        }

        private void Disable()
        {
            MovementReference.CanMove = false;
            TopWallDetectorReference.Enabled = false;
            BottomWallDetectorReference.Enabled = false;
            HitboxReference.SetDeferred("disabled", true);
            VisualReference.Visible = false;
            Enabled = false;
        }
    }
}