using System.Collections.Generic;
using Godot;

namespace Game
{

    public class JinglePlayerImpl : JinglePlayer
    {
        private Dictionary<JingleType, AudioStream> _jingles = new Dictionary<JingleType, AudioStream>()
        {
            {JingleType.Starman, null},
            {JingleType.CourseClear, null},
            {JingleType.CastleClear, null},
            {JingleType.Ending, null},
            {JingleType.Death, null},
            {JingleType.GameOver, null}
        };
        private JingleType _currentJingle = JingleType.Ending;

        public override void PlayJingle(JingleType jingleToPlay)
        {
            if (_currentJingle == jingleToPlay)
            {
                Play();
                return;
            }
            switch (jingleToPlay)
            {
                case JingleType.Starman:
                    LoadStarmanTheme();
                    _currentJingle = JingleType.Starman;
                    Stream = _jingles[JingleType.Starman];
                    break;
                case JingleType.CourseClear:
                    break;
                case JingleType.CastleClear:
                    break;
                case JingleType.Ending:
                    break;
                case JingleType.Death:
                    LoadDeathJingle();
                    _currentJingle = JingleType.Death;
                    Stream = _jingles[JingleType.Death];
                    break;
                case JingleType.GameOver:
                    break;
            }
            Play();
        }

        private void LoadStarmanTheme()
        {
            if (_jingles[JingleType.Starman] != null)
            {
                return;
            }
            _jingles[JingleType.Starman] = GD.Load<AudioStream>("res://Items/Audio/starman.wav");
        }

        private void LoadDeathJingle()
        {
            if (_jingles[JingleType.Death] != null)
            {
                return;
            }
            _jingles[JingleType.Death] = GD.Load<AudioStream>("res://Game/Audio/death.wav");
        }

        public override void StopJingle()
        {
            Stop();
        }

        public override void OnJingleFinished()
        {
            throw new System.NotImplementedException();
        }
    }
}