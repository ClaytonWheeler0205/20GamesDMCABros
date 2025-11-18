using Godot;

namespace Game.Player
{

    public class JumpHitbox : Area2D
    {
        private float _verticalVelocity;
        public float VerticalVelocity
        {
            get { return _verticalVelocity; }
            set { _verticalVelocity = value; }
        }
        private float _yPosition;
        public float YPosition
        {
            get { return _yPosition; }
            set { _yPosition = value; }
        }
        private bool _hasHitBlock = false;
        public bool HasHitBlock
        {
            get { return _hasHitBlock; }
            set { _hasHitBlock = value; }
        }
    }
}