namespace Game.Player
{

    public class StarComponentImpl : StarComponent
    {
        public override void StartTimers()
        {
            InvincibilityTimeReference.Start(InvincibilityTime);
            MusicTimeReference.Start(MusicTime);
            FastFlashTimeReference.Start(FastFlashTime);
        }

        public override void OnInvincibilityTimeTimeout()
        {
            PaletteAnimatior.PlaybackSpeed = 1.0f;
            PaletteAnimatior.Stop();
            PlayerPalette.SetPlayerColor(-1);
        }

        public override void OnFastFlashTimeTimeout()
        {
            PaletteAnimatior.PlaybackSpeed = 0.75f;
        }
    }
}