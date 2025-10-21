using Godot;

namespace Game.Player
{

    public class JumpHitbox : Area2D
    {
        private Size _playerSize;
        public Size PlayerSize
        {
            get { return _playerSize; }
            set { _playerSize = value; }
        }
        private float _verticalVelocity;
        public float VerticalVelocity
        {
            get { return _verticalVelocity; }
            set { _verticalVelocity = value; }
        }
        private bool _hasHitBlock = false;
        public bool HasHitBlock
        {
            get { return _hasHitBlock; }
            set { _hasHitBlock = value;}
        }
    }
}