using System.Collections.Generic;
using Game.Buses;
using Godot;

namespace Game
{

    public class JinglePlayerImpl : JinglePlayer
    {
        private Dictionary<JingleType, AudioStream> _jingles = new Dictionary<JingleType, AudioStream>()
        {
            {JingleType.Starman, null},
            {JingleType.StarmanFast, null},
            {JingleType.CourseClear, null},
            {JingleType.CastleClear, null},
            {JingleType.Ending, null},
            {JingleType.Death, null},
            {JingleType.GameOver, null},
            {JingleType.Hurry, null}
        };
        private JingleType _currentJingle = JingleType.Ending;

        public override void PlayJingle(JingleType jingleToPlay)
        {
            StreamPaused = false;
            if (_currentJingle == jingleToPlay)
            {
                Play();
                return;
            }
            switch (jingleToPlay)
            {
                case JingleType.Starman:
                    LoadStarmanTheme();
                    break;
                case JingleType.CourseClear:
                    LoadCourseClearJingle();
                    _currentJingle = JingleType.CourseClear;
                    Stream = _jingles[JingleType.CourseClear];
                    HurryJinglePlayed = false;
                    break;
                case JingleType.CastleClear:
                    HurryJinglePlayed = false;
                    break;
                case JingleType.Ending:
                    break;
                case JingleType.Death:
                    LoadDeathJingle();
                    _currentJingle = JingleType.Death;
                    Stream = _jingles[JingleType.Death];
                    HurryJinglePlayed = false;
                    break;
                case JingleType.GameOver:
                    LoadGameOverJingle();
                    _currentJingle = JingleType.GameOver;
                    Stream = _jingles[JingleType.GameOver];
                    break;
                case JingleType.Hurry:
                    LoadHurryJingle();
                    _currentJingle = JingleType.Hurry;
                    Stream = _jingles[JingleType.Hurry];
                    HurryJinglePlayed = true;
                    break;
            }
            Play();
        }

        private void LoadStarmanTheme()
        {
            if (HurryJinglePlayed)
            {
                LoadFastStarmanTheme();
                return;
            }
            LoadStandardStarmanTheme();
        }

        private void LoadStandardStarmanTheme()
        {
            if (_jingles[JingleType.Starman] != null)
            {
                _currentJingle = JingleType.Starman;
                Stream = _jingles[JingleType.Starman];
                return;
            }
            _jingles[JingleType.Starman] = GD.Load<AudioStream>("res://Items/Audio/starman.wav");
            _currentJingle = JingleType.Starman;
            Stream = _jingles[JingleType.Starman];
        }

        private void LoadFastStarmanTheme()
        {
            if (_jingles[JingleType.StarmanFast] != null)
            {
                return;
            }
            _jingles[JingleType.StarmanFast] = GD.Load<AudioStream>("res://Items/Audio/starman_low_time.wav");
            _currentJingle = JingleType.StarmanFast;
            Stream = _jingles[JingleType.StarmanFast];
        }

        private void LoadCourseClearJingle()
        {
            if (_jingles[JingleType.CourseClear] != null)
                return;
            _jingles[JingleType.CourseClear] = GD.Load<AudioStream>("res://Game/Audio/course_clear.wav");
        }

        private void LoadDeathJingle()
        {
            if (_jingles[JingleType.Death] != null)
            {
                return;
            }
            _jingles[JingleType.Death] = GD.Load<AudioStream>("res://Game/Audio/death.wav");
        }

        private void LoadGameOverJingle()
        {
            if (_jingles[JingleType.GameOver] != null)
            {
                return;
            }
            _jingles[JingleType.GameOver] = GD.Load<AudioStream>("res://Game/Audio/game_over.wav");
        }

        private void LoadHurryJingle()
        {
            if (_jingles[JingleType.Hurry] != null)
            {
                return;
            }
            _jingles[JingleType.Hurry] = GD.Load<AudioStream>("res://Game/Audio/hurry.wav");
        }

        public override void StopJingle()
        {
            _currentJingle = JingleType.None;
            Stop();
        }

        public override void OnJingleFinished()
        {
            if (_currentJingle == JingleType.Death)
            {
                LivesEventBus.Instance.EmitSignal("LifeLost");
            }
            else if (_currentJingle == JingleType.Hurry)
            {
                LevelEventBus.Instance.EmitSignal("HurryJingleFinished");
            }
        }

        public override void OnStarFinished()
        {
            if (_currentJingle == JingleType.Starman || _currentJingle == JingleType.StarmanFast)
                Stop();
        }
    }
}
