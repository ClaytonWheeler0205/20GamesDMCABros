using Game.Buses;
using Godot;

namespace Game.Levels
{

    public abstract class Level : Node
    {
        [Export]
        private NodePath _musicPlayerPath;
        private LevelMusicPlayer _musicPlayerReference;
        protected LevelMusicPlayer MusicPlayerReference
        {
            get { return _musicPlayerReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _musicPlayerReference = GetNode<LevelMusicPlayer>(_musicPlayerPath);
        }

        public abstract void Start();
    }
}