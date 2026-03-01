namespace Game.Levels
{

    public class LevelMusicPlayerImpl : LevelMusicPlayer
    {
        public override void StartLevelMusic()
        {
            if (InSubworld)
            {
                PlaySubworldMusic();
            }
            else
            {
                PlayWorldMusic();
            }
        }

        public override void SwitchMusic()
        {
            if (InSubworld)
            {
                PlayWorldMusic();
            }
            else
            {
                PlaySubworldMusic();
            }
        }

        private void PlayWorldMusic()
        {
            if (IsLowTime)
            {
                FastWorldMusicPlayerReference.Play();
                return;
            }
            WorldMusicPlayerReference.Play();
        }

        private void PlaySubworldMusic()
        {
            if (IsLowTime)
            {
                FastSubworldMusicPlayerReference.Play();
                return;
            }
            SubworldMusicPlayerReference.Play();
        }

        public override void StopMusic()
        {
            WorldMusicPlayerReference.Stop();
            FastWorldMusicPlayerReference.Stop();
            SubworldMusicPlayerReference.Stop();
            FastSubworldMusicPlayerReference.Stop();
        }

        public override void OnPlayerDied()
        {
            StopMusic();
            JinglePlayer.Instance.PlayJingle(JingleType.Death);
        }

        public override void OnStarCollected()
        {
            StopMusic();
            ShouldStarmanThemePlay = true;
        }

        public override void OnStarEnding()
        {
            StartLevelMusic();
            ShouldStarmanThemePlay = false;
        }

        public override void OnTimeLow()
        {
            StopMusic();
            JinglePlayer.Instance.PlayJingle(JingleType.Hurry);
            IsLowTime = true;
        }

        public override void OnHurryJingleFinished()
        {
            if (ShouldStarmanThemePlay)
            {
                JinglePlayer.Instance.PlayJingle(JingleType.Starman);
                return;
            }
            StartLevelMusic();
        }
    }
}