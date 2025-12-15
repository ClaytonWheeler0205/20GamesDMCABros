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
        private NodePath _enemyHitboxAreaPath;
        private Area2D _enemyHitboxAreaReference;
        public Area2D EnemyHitboxAreaReference
        {
            get { return _enemyHitboxAreaReference; }
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
            _enemyHitboxAreaReference = GetNode<Area2D>(_enemyHitboxAreaPath);
        }

        public abstract void OnBodyEntered(Node body);
        public abstract void OnAreaEntered(Area2D area);
        public abstract void OnScreenEntered();
        public abstract void OnScreenExited();
    }
}