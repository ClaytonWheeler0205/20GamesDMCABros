using System.Collections.Generic;
using Game.Buses;
using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public enum LevelTheme
    {
        Overworld,
        Underground,
        Castle,
        Underwater
    }

    public enum Jingle
    {
        Death,
        GameOver,
        LevelClear,
        CastleClear
    }

    public class MusicPlayer : Node
    {

        [Export]
        private NodePath _musicPlayerPath;
        private static AudioStreamPlayer _musicPlayer;
        [Export]
        private NodePath _jinglePlayerPath;
        private static AudioStreamPlayer _jinglePlayer;
        private static Dictionary<LevelTheme, AudioStream> _levelSongs = new Dictionary<LevelTheme, AudioStream>()
        {
            {LevelTheme.Overworld, null},
            {LevelTheme.Underground, null},
            {LevelTheme.Castle, null},
            {LevelTheme.Underwater, null}
        };
        private static Dictionary<Jingle, AudioStream> _jingles = new Dictionary<Jingle, AudioStream>()
        {
            {Jingle.Death, null},
            {Jingle.GameOver, null},
            {Jingle.LevelClear, null},
            {Jingle.CastleClear, null}
        };
        private static AudioStream _starman = null;
        private static AudioStream _ending = null;
        private static MusicPlayer _instance;
        public static MusicPlayer Instance
        {
            get { return _instance; }
        }

        public override void _Ready()
        {
            if (_instance != null && _instance != this)
            {
                this.SafeQueueFree();
                return;
            }
            _instance = this;
            SetNodeReferences();
            SetNodeConnections();
            PlayLevelMusic(LevelTheme.Overworld);
        }

        private void SetNodeReferences()
        {
            _musicPlayer = GetNode<AudioStreamPlayer>(_musicPlayerPath);
            _jinglePlayer = GetNode<AudioStreamPlayer>(_jinglePlayerPath);
        }

        private void SetNodeConnections()
        {
            PowerupEventBus.Instance.Connect("StarCollected", this, nameof(OnStarCollected));
        }

        public void PlayLevelMusic(LevelTheme theme)
        {
            switch (theme)
            {
                case LevelTheme.Overworld:
                    LoadOverworldMusic();
                    _musicPlayer.Stream = _levelSongs[LevelTheme.Overworld];
                    break;
                case LevelTheme.Underground:
                    break;
                case LevelTheme.Castle:
                    break;
                case LevelTheme.Underwater:
                    break;
            }
            _musicPlayer.Play();
        }

        private void LoadOverworldMusic()
        {
            if (_levelSongs[LevelTheme.Overworld] != null)
            {
                return;
            }
            _levelSongs[LevelTheme.Overworld] = GD.Load<AudioStream>("res://Levels/Audio/overworld.wav");
        }

        public void PlayJingle(Jingle jingle)
        {
            _musicPlayer.Stop();
            switch (jingle)
            {
                case Jingle.LevelClear:
                    break;
                case Jingle.CastleClear:
                    break;
                case Jingle.Death:
                    LoadDeathJingle();
                    _jinglePlayer.Stream = _jingles[Jingle.Death];
                    break;
                case Jingle.GameOver:
                    break;
            }
            _jinglePlayer.Play();
        }

        private void LoadDeathJingle()
        {
            if (_jingles[Jingle.Death] != null)
            {
                return;
            }
            _jingles[Jingle.Death] = GD.Load<AudioStream>("res://Game/Audio/death.wav");
        }

        public void OnStarCollected()
        {
            PlayStarmanTheme();
        }

        private void PlayStarmanTheme()
        {
            LoadStarmanTheme();
            _musicPlayer.Stream = _starman;
            _musicPlayer.Play();
        }

        private void LoadStarmanTheme()
        {
            if (_starman != null)
            {
                return;
            }
            _starman = GD.Load<AudioStream>("res://Items/Audio/starman.wav");
        }
    }
}