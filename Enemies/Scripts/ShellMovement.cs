using Godot;

namespace Game.Enemies
{

    public class ShellMovement : EnemyMovement
    {
        [Export]
        private NodePath _shellBounceSoundPath;
        private AudioStreamPlayer _shellBounceSoundReference;

        public override void _Ready()
        {
            base._Ready();
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _shellBounceSoundReference = GetNode<AudioStreamPlayer>(_shellBounceSoundPath);
        }

        public override void FlipDirection()
        {
            base.FlipDirection();
            if (ShouldMove)
                _shellBounceSoundReference.Play();
        }

    }
}