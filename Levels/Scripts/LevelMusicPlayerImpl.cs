namespace Game.Levels
{

    public class LevelMusicPlayerImpl : LevelMusicPlayer
    {
        public override void StartLevelMusic()
        {
            if (InSubworld)
            {
                SubworldMusicPlayerReference.Play();
            }
            else
            {
                WorldMusicPlayerReference.Play();
            }
        }

        public override void SwitchMusic()
        {
            if (InSubworld)
            {
                WorldMusicPlayerReference.Play();
            }
            else
            {
                SubworldMusicPlayerReference.Play();
            }
        }

        public override void StopMusic()
        {
            WorldMusicPlayerReference.Stop();
            SubworldMusicPlayerReference.Stop();
        }

        public override void OnPlayerDied()
        {
            StopMusic();
            JinglePlayer.Instance.PlayJingle(JingleType.Death);
        }

        public override void OnStarCollected()
        {
            StopMusic();
        }

        public override void OnStarEnding()
        {
            StartLevelMusic();
        }
    }
}