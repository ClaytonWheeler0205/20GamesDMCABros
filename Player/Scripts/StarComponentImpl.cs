using Game.Buses;

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

        public override void ForceStopinvincibility()
        {
            InvincibilityTimeReference.Stop();
            MusicTimeReference.Stop();
            FastFlashTimeReference.Stop();
            PaletteAnimatior.PlaybackSpeed = 1.0f;
            PaletteAnimatior.Stop();
        }

        public override void OnInvincibilityTimeTimeout()
        {
            PaletteAnimatior.PlaybackSpeed = 1.0f;
            PaletteAnimatior.Stop();
            PlayerPalette.SetPlayerColor(-1);
        }

        public override void OnMusicTimeTimeout()
        {
            PowerupEventBus.Instance.EmitSignal("StarEnding");
            JinglePlayer.Instance.StopJingle();
        }

        public override void OnFastFlashTimeTimeout()
        {
            PaletteAnimatior.PlaybackSpeed = 0.75f;
        }
    }
}