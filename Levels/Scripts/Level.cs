using Godot;

namespace Game.Levels
{
    public enum LevelType
    {
        Overworld = 0,
        Underground = 1,
        Castle = 2,
        Underwater = 3
    }

    public abstract class Level : Node2D
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
        private Node2D _coinContainerReference;
        protected Node2D CoinContainerReference
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
        private Vector2 _cameraPointPosition;
        public Vector2 CameraPointPosition
        {
            get { return _cameraPointPosition; }
            set { _cameraPointPosition = value; }
        }
        private Vector2 _cameraStartPosition;
        protected Vector2 CameraStartPosition
        {
            get { return _cameraStartPosition; }
        }
        [Export]
        private LevelType _worldType;
        protected LevelType WorldType
        {
            get { return _worldType; }
        }
        [Export]
        private LevelType _subworldType;
        protected LevelType SubworldType
        {
            get { return _subworldType; }
        }
        private bool _inSubworld = false;
        protected bool InSubworld
        {
            get { return _inSubworld; }
            set { _inSubworld = value; }
        }
        private bool _lastCheckpointInSubworld;
        protected bool LastCheckpointInSubworld
        {
            get { return _lastCheckpointInSubworld; }
            set { _lastCheckpointInSubworld = value; }
        }
        private ShaderMaterial _paletteMaterial;
        protected ShaderMaterial PaletteMaterial
        {
            get { return _paletteMaterial; }
        }
        private ShaderMaterial _coinsMaterial;
        protected ShaderMaterial CoinsMaterial
        {
            get { return _coinsMaterial; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            _paletteMaterial = (ShaderMaterial)Material;
            _coinsMaterial = (ShaderMaterial)_coinContainerReference.Material;
            _levelStartPosition = _startingPointReference.GlobalPosition;
            _cameraStartPosition = new Vector2(_startingPointReference.GlobalPosition.x + 70.0f, _startingPointReference.GlobalPosition.y - 96.0f);
            _cameraPointPosition = _cameraStartPosition;
        }

        private void SetNodeReferences()
        {
            _musicPlayerReference = GetNode<LevelMusicPlayer>(_musicPlayerPath);
            _enemyContainerReference = GetNode<Node>(_enemyContainerPath);
            _blockContainerReference = GetNode<Node>(_blockContainerPath);
            _coinContainerReference = GetNode<Node2D>(_coinContainerPath);
            _checkpointContainerReference = GetNode<Node>(_checkpointContainerPath);
            _deathPitsReference = GetNode<Node>(_deathPitsPath);
            _startingPointReference = GetNode<Position2D>(_startingPointPath);
        }

        private void SetNodeConnections()
        {
            SetCheckpointConnections();
            LevelEventBus.Instance.Connect("PipeTransitionFinished", this, nameof(OnPipeTransitionFinished));
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
        public abstract void OnPlayerReachedCheckpoint(Vector2 checkpointPosition, Vector2 cameraPosition, bool lastCheckpointInSubworld);
        public abstract void OnPipeTransitionFinished(bool playExitAnimation);
    }
}