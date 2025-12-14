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
        private NodePath _subworldMusicPlayerPath;
        private AudioStreamPlayer _subworldMusicPlayerReference;
        protected AudioStreamPlayer SubworldMusicPlayerReference
        {
            get { return _subworldMusicPlayerReference; }
        }
        private bool _inSubworld = false;
        protected bool InSubworld
        {
            get { return _inSubworld; }
            set { _inSubworld = value; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _worldMusicPlayerReference = GetNode<AudioStreamPlayer>(_worldMusicPlayerPath);
            _subworldMusicPlayerReference = GetNode<AudioStreamPlayer>(_subworldMusicPlayerPath);
        }

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("PlayerDied", this, nameof(OnPlayerDied));
            PowerupEventBus.Instance.Connect("StarCollected", this, nameof(OnStarCollected));
            PowerupEventBus.Instance.Connect("StarEnding", this, nameof(OnStarEnding));
        }

        public abstract void StartLevelMusic();
        public abstract void SwitchMusic();
        public abstract void StopMusic();
        public abstract void OnPlayerDied();
        public abstract void OnStarCollected();
        public abstract void OnStarEnding();
    }
}