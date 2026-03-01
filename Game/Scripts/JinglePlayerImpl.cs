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
                    LoadGameOverJingle();
                    _currentJingle = JingleType.GameOver;
                    Stream = _jingles[JingleType.GameOver];
                    break;
                case JingleType.Hurry:
                    LoadHurryJingle();
                    _currentJingle = JingleType.Hurry;
                    Stream = _jingles[JingleType.Hurry];
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
    }
}