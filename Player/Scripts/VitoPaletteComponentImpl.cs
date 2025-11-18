namespace Game.Player
{

    public class VitoPaletteComponentImpl : VitoPaletteComponent
    {
        public override void SetPlayerColor(int paletteCode)
        {
            if (paletteCode < 0)
            {
                PlayerMaterial.SetShaderParam("palette_code", (int)CurrentPlayerColor);
                return;
            }
            PlayerMaterial.SetShaderParam("palette_code", paletteCode);
        }

        public override void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "fire_transform")
            {
                PlayerMaterial.SetShaderParam("palette_code", PaletteCode.Fire);
                CurrentPlayerColor = PaletteCode.Fire;
                GetTree().Paused = false;
                ResetInvincibilityAnimation();
            }
        }

        private void ResetInvincibilityAnimation()
        {
            if (IsInvincibilityTimerRunning)
            {
                if (IsFastFlashTimerRunning)
                {
                    PaletteAnimator.PlaybackSpeed = 2.0f;
                }
                else
                {
                    PaletteAnimator.PlaybackSpeed = 0.75f;
                }
                PaletteAnimator.Play("invincibility_flash");
            }
        }
    }
}