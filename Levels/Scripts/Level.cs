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
        [Export]
        private NodePath _startingPointPath;
        private Position2D _startingPointReference;
        public Position2D StartingPointReference
        {
            get { return _startingPointReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _musicPlayerReference = GetNode<LevelMusicPlayer>(_musicPlayerPath);
            _startingPointReference = GetNode<Position2D>(_startingPointPath);
        }

        public abstract void Start();
        public abstract Vector2 GetPlayerSpawnPoint();
        public abstract void ResetEnemies();
    }
}