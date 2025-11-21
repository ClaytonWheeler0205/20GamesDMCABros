using Godot;

namespace Game.Enemies
{

    public class EnemyMovement : BasicMovementImpl
    {
        [Export]
        private Vector2 _rightRayLocation;
        [Export]
        private Vector2 _leftRayLocation;

        public override void _Ready()
        {
            base._Ready();
            SetRayDirection();
        }

        private void SetRayDirection()
        {
            switch (MovementDirection)
            {
                case Direction.Right:
                    WallDetectorReference.Position = _rightRayLocation;
                    break;
                case Direction.Left:
                    WallDetectorReference.Position = _leftRayLocation;
                    break;
            }
        }

        public override void FlipDirection()
        {
            base.FlipDirection();
            SetRayDirection();
        }

    }
}