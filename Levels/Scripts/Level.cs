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
        private NodePath _enemyContainerPath;
        private Node _enemyContainerReference;
        protected Node EnemyContainerReference
        {
            get { return _enemyContainerReference; }
        }
        [Export]
        private NodePath _blockContainerPath;
        private Node _blockContainerReference;
        protected Node BlockContainerReference
        {
            get { return _blockContainerReference; }
        }
        [Export]
        private NodePath _coinContainerPath;
        private Node _coinContainerReference;
        protected Node CoinContainerReference
        {
            get { return _coinContainerReference; }
        }
        [Export]
        private NodePath _deathPitsPath;
        private Node _deathPitsReference;
        protected Node DeathPitsReference
        {
            get { return _deathPitsReference; }
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
            _enemyContainerReference = GetNode<Node>(_enemyContainerPath);
            _blockContainerReference = GetNode<Node>(_blockContainerPath);
            _coinContainerReference = GetNode<Node>(_coinContainerPath);
            _deathPitsReference = GetNode<Node>(_deathPitsPath);
            _startingPointReference = GetNode<Position2D>(_startingPointPath);
        }

        public abstract void Start(bool firstLoad);
        public abstract Vector2 GetPlayerSpawnPoint();
        public abstract void ResetLevel();
    }
}