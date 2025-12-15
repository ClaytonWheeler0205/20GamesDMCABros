using Godot;

namespace Game.Enemies
{

    public abstract class Enemy : KinematicBody2D
    {

        [Export]
        private NodePath _enemyVisualPath;
        private AnimatedSprite _enemyVisualReference;
        protected AnimatedSprite EnemyVisualReference
        {
            get { return _enemyVisualReference; }
        }
        [Export]
        private NodePath _enemyScreenDetectorPath;
        private VisibilityNotifier2D _enemyScreenDetectorReference;
        protected VisibilityNotifier2D EnemyScreenDetectorReference
        {
            get { return _enemyScreenDetectorReference; }
        }
        [Export]
        private NodePath _physicalHitboxPath;
        private CollisionShape2D _physicalHitboxReference;
        protected CollisionShape2D PhysicalHitboxReference
        {
            get { return _physicalHitboxReference; }
        }
        [Export]
        private NodePath _enemyHitboxAreaPath;
        private Area2D _enemyHitboxAreaReference;
        public Area2D EnemyHitboxAreaReference
        {
            get { return _enemyHitboxAreaReference; }
        }
        [Export]
        private NodePath _enemyHitboxPath;
        private CollisionShape2D _enemyHitboxReference;
        protected CollisionShape2D EnemyHitboxReference
        {
            get { return _enemyHitboxReference; }
        }
        private Vector2 _startingPosition;
        public Vector2 StartingPosition
        {
            get { return _startingPosition; }
        }

        public override void _Ready()
        {
            _startingPosition = GlobalPosition;
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _enemyVisualReference = GetNode<AnimatedSprite>(_enemyVisualPath);
            _enemyScreenDetectorReference = GetNode<VisibilityNotifier2D>(_enemyScreenDetectorPath);
            _physicalHitboxReference = GetNode<CollisionShape2D>(_physicalHitboxPath);
            _enemyHitboxAreaReference = GetNode<Area2D>(_enemyHitboxAreaPath);
            _enemyHitboxReference = GetNode<CollisionShape2D>(_enemyHitboxPath);
        }

        public abstract void EnableEnemy();
        public abstract void DisableEnemy();
        public abstract void OnBodyEntered(Node body);
        public abstract void OnAreaEntered(Area2D area);
        public abstract void OnScreenEntered();
        public abstract void OnScreenExited();
    }
}