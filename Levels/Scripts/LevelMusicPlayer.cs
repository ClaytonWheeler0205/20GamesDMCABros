using Game.Buses;
using Godot;

namespace Game.Levels
{

    public abstract class LevelMusicPlayer : Node
    {
        [Export]
        private NodePath _worldMusicPlayerPath;
        private AudioStreamPlayer _worldMusicPlayerReference;
        protected AudioStreamPlayer WorldMusicPlayerReference
        {
            get { return _worldMusicPlayerReference; }
        }
        [Export]
        private NodePath _fastWorldMusicPlayerPath;
        private AudioStreamPlayer _fastWorldMusicPlayerReference;
        protected AudioStreamPlayer FastWorldMusicPlayerReference
        {
            get { return _fastWorldMusicPlayerReference; }
        }
        [Export]
        private NodePath _subworldMusicPlayerPath;
        private AudioStreamPlayer _subworldMusicPlayerReference;
        protected AudioStreamPlayer SubworldMusicPlayerReference
        {
            get { return _subworldMusicPlayerReference; }
        }
        [Export]
        private NodePath _fastSubworldMusicPlayerPath;
        private AudioStreamPlayer _fastSubworldMusicPlayerReference;
        protected AudioStreamPlayer FastSubworldMusicPlayerReference
        {
            get { return _fastSubworldMusicPlayerReference; }
        }
        private bool _inSubworld = false;
        protected bool InSubworld
        {
            get { return _inSubworld; }
            set { _inSubworld = value; }
        }
        private bool _isLowTime = false;
        public bool IsLowTime
        {
            protected get { return _isLowTime; }
            set { _isLowTime = value; }
        }
        private bool _shouldStarmanThemePlay;
        protected bool ShouldStarmanThemePlay
        {
            get { return _shouldStarmanThemePlay; }
            set { _shouldStarmanThemePlay = value; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _worldMusicPlayerReference = GetNode<AudioStreamPlayer>(_worldMusicPlayerPath);
            _fastWorldMusicPlayerReference = GetNode<AudioStreamPlayer>(_fastWorldMusicPlayerPath);
            _subworldMusicPlayerReference = GetNode<AudioStreamPlayer>(_subworldMusicPlayerPath);
            _fastSubworldMusicPlayerReference = GetNode<AudioStreamPlayer>(_fastSubworldMusicPlayerPath);
        }

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("PlayerDied", this, nameof(OnPlayerDied));
            PowerupEventBus.Instance.Connect("StarCollected", this, nameof(OnStarCollected));
            PowerupEventBus.Instance.Connect("StarEnding", this, nameof(OnStarEnding));
            TimerEventBus.Instance.Connect("TimeLow", this, nameof(OnTimeLow));
            LevelEventBus.Instance.Connect("HurryJingleFinished", this, nameof(OnHurryJingleFinished));
        }

        public abstract void StartLevelMusic();
        public abstract void SwitchMusic();
        public abstract void StopMusic();
        public abstract void OnPlayerDied();
        public abstract void OnStarCollected();
        public abstract void OnStarEnding();
        public abstract void OnTimeLow();
        public abstract void OnHurryJingleFinished();
    }
}