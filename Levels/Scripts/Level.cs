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
        private NodePath _checkpointContainerPath;
        private Node _checkpointContainerReference;
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
        private Vector2 _levelStartPosition;
        protected Vector2 LevelStartPosition
        {
            get { return _levelStartPosition; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _musicPlayerReference = GetNode<LevelMusicPlayer>(_musicPlayerPath);
            _enemyContainerReference = GetNode<Node>(_enemyContainerPath);
            _blockContainerReference = GetNode<Node>(_blockContainerPath);
            _coinContainerReference = GetNode<Node>(_coinContainerPath);
            _checkpointContainerReference = GetNode<Node>(_checkpointContainerPath);
            _deathPitsReference = GetNode<Node>(_deathPitsPath);
            _startingPointReference = GetNode<Position2D>(_startingPointPath);
            _levelStartPosition = _startingPointReference.GlobalPosition;
        }

        private void SetNodeConnections()
        {
            SetCheckpointConnections();
        }

        private void SetCheckpointConnections()
        {
            foreach (Node node in _checkpointContainerReference.GetChildren())
            {
                if (node.IsInGroup("checkpoint"))
                {
                    node.Connect("PlayerReachedCheckpoint", this, nameof(OnPlayerReachedCheckpoint));
                }
            }
        }

        // TODO: Remove the bool parameter. Split these into two different method calls
        public abstract void Start(bool firstLoad);
        public abstract Vector2 GetPlayerSpawnPoint();
        public abstract void ResetPlayerSpawnPoint();
        public abstract void ResetLevel();
        public abstract void OnPlayerReachedCheckpoint(Vector2 checkpointPosition);
    }
}