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

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _enemyVisualReference = GetNode<AnimatedSprite>(_enemyVisualPath);
        }

        public abstract void OnBodyEntered(Node body);
        public abstract void OnScreenEntered();
        public abstract void OnScreenExited();
    }
}