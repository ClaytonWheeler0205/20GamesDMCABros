using Game.Player;
using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public class Camera : Node2D
    {
        private Viewport _cameraViewport;
        private Vector2 _screenSize;
        private float _scrollSpeed;
        private bool _shouldScroll = false;
        private Vito _followTarget;

        public override void _Ready()
        {
            _cameraViewport = GetViewport();
            _screenSize = GetViewportRect().Size;
            ApplyCameraPosition();
        }

        private void ApplyCameraPosition()
        {
            Transform2D transform = _cameraViewport.CanvasTransform;
            transform.origin = -Position + (_screenSize / 2);
            _cameraViewport.CanvasTransform = transform;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (!_shouldScroll)
            {
                return;
            }
            _scrollSpeed = _followTarget.GetVelocityVector().x;
            if (_scrollSpeed <= 0.0f)
            {
                return;
            }
            Vector2 targetPos = new Vector2(_followTarget.GlobalPosition.x + 16.0f, Position.y);
            Position = Position.MoveToward(targetPos, delta * _scrollSpeed);
            ApplyCameraPosition();
        }


        public void OnBodyEntersArea(Node body)
        {
            if (!body.IsInGroup("player"))
            {
                return;
            }
            if (!(body is Vito vito))
            {
                return;
            }
            else
            {
                Assigntarget(vito);
            }

            _shouldScroll = true;
        }

        private void Assigntarget(Vito target)
        {
            if (_followTarget.IsValid())
            {
                return;
            }
            _followTarget = target;
        }

        public void OnBodyExitsArea(Node body)
        {
            if (!body.IsInGroup("player"))
            {
                return;
            }

            _shouldScroll = false;
        }

    }
}