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
        private bool _hasHitBlock = false;
        public bool HasHitBlock
        {
            get { return _hasHitBlock; }
            set { _hasHitBlock = value;}
        }
    }
}