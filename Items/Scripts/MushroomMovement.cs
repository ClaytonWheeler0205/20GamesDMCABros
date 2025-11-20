using Godot;

namespace Game.Items
{

    public abstract class MushroomMovement : BasicMovementImpl
    {
        [Export]
        private float _bounceForce = -50.0f;
        protected float BounceForce
        {
            get { return _bounceForce; }
        }

        public abstract void Bounce();
    }
}