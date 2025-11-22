using System;
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

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _enemyVisualReference = GetNode<AnimatedSprite>(_enemyVisualPath);
            _enemyScreenDetectorReference = GetNode<VisibilityNotifier2D>(_enemyScreenDetectorPath);
        }

        public abstract void OnBodyEntered(Node body);
        public abstract void OnAreaEntered(Area2D area);
        public abstract void OnScreenEntered();
        public abstract void OnScreenExited();
    }
}