using Godot;
using System.Collections.Generic;

namespace Game.Projectiles
{

    public abstract class FireballFactory : Node2D
    {
        private PackedScene _fireballScene = GD.Load<PackedScene>("res://Projectiles/Scenes/Fireball.tscn");
        private List<Fireball> _fireballs = new List<Fireball>(2);
        protected List<Fireball> Fireballs
        {
            get { return _fireballs; }
        }

        public override void _Ready()
        {
            CreateFireballPool();
        }

        private void CreateFireballPool()
        {
            for (int i = 0; i < _fireballs.Capacity; i++)
            {
                Fireball fireball = _fireballScene.Instance<Fireball>();
                GetTree().Root.CallDeferred("add_child", fireball);
                _fireballs.Add(fireball);
            }
        }

        public abstract Fireball GetFireball();
    }
}