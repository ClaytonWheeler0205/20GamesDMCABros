using Game.Buses;
using Godot;

namespace Game.Player
{

    public class SuperPlayerAnimator : Node2D, PlayerAnimator
    {
        public Vito PlayerToAnimate { get; set; }
        public MovementComponent PlayerMovement { get; set; }
        [Export]
        private float _minimumSpeedForMovement = 10.0f;
        [Export]
        private NodePath _topPartToAnimatePath;
        private AnimatedSprite _topPartToAnimateReference;
        [Export]
        private NodePath _bottomPartToAnimatePath;
        private AnimatedSprite _bottomPartToAnimateReference;
        private Vector2 _growShrinkSpriteOffset = new Vector2(0, -12);
        private bool _isGrowingOrShrinking = false;

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _topPartToAnimateReference = GetNode<AnimatedSprite>(_topPartToAnimatePath);
            _bottomPartToAnimateReference = GetNode<AnimatedSprite>(_bottomPartToAnimatePath);
        }

        private void SetNodeConnections()
        {
            PowerupEventBus.Instance.Connect("MushroomCollected", this, nameof(OnMushroomCollected));
        }

        public override void _Process(float delta)
        {
            if (!Visible || _isGrowingOrShrinking)
            {
                return;
            }
            AnimatePlayer();
        }

        private void AnimatePlayer()
        {
            string animationToPlay;
            if (PlayerToAnimate.IsOnFloor())
            {
                FlipToCurrentDirection();
                float playerHorizontalSpeed = PlayerToAnimate.GetVelocityVector().x;
                if (PlayerMovement.IsCrouched)
                {
                    if (PlayerMovement.Direction == 0.0f)
                    {
                        animationToPlay = "crouch";
                    }
                    else
                    {
                        animationToPlay = "idle";
                    }
                }
                else if (Mathf.Abs(playerHorizontalSpeed) < _minimumSpeedForMovement)
                {
                    animationToPlay = "idle";
                }
                else if (PlayerMovement.IsSkidding)
                {
                    animationToPlay = "skid";
                }
                else
                {
                    if (PlayerMovement.IsRunning)
                    {
                        _topPartToAnimateReference.SpeedScale = 1.5f;
                        _bottomPartToAnimateReference.SpeedScale = 1.5f;
                    }
                    else
                    {
                        _topPartToAnimateReference.SpeedScale = 1.0f;
                        _bottomPartToAnimateReference.SpeedScale = 1.0f;
                    }
                    animationToPlay = "walk";
                }
            }
            else
            {
                if (_topPartToAnimateReference.Animation == "crouch")
                {
                    animationToPlay = "crouch";
                }
                else
                {
                    animationToPlay = "jump";
                }
            }
            _topPartToAnimateReference.Play(animationToPlay);
            _bottomPartToAnimateReference.Play(animationToPlay);
        }

        private void FlipToCurrentDirection()
        {
            float horizontalDirection = PlayerToAnimate.GetVelocityVector().x;
            if (horizontalDirection > 0.0f)
            {
                _topPartToAnimateReference.FlipH = false;
                _bottomPartToAnimateReference.FlipH = false;
            }
            else if (horizontalDirection < 0.0f)
            {
                _topPartToAnimateReference.FlipH = true;
                _bottomPartToAnimateReference.FlipH = true;
            }
        }

        public void ToggleAnimation()
        {
            Visible = !Visible;
            if (Visible)
            {
                return;
            }
            _topPartToAnimateReference.Stop();
            _bottomPartToAnimateReference.Stop();
        }

        public void OnMushroomCollected()
        {
            PauseMode = PauseModeEnum.Process;
            _isGrowingOrShrinking = true;
            _bottomPartToAnimateReference.Offset = _growShrinkSpriteOffset;
            _bottomPartToAnimateReference.Play("grow");
            GetTree().Paused = true;
        }

        public void OnAnimationFinished()
        {
            if (_bottomPartToAnimateReference.Animation != "grow")
            {
                return;
            }
            _isGrowingOrShrinking = false;
            _bottomPartToAnimateReference.Offset = Vector2.Zero;
            _topPartToAnimateReference.Visible = true;
            PauseMode = PauseModeEnum.Stop;
            GetTree().Paused = false;
        }
    }
}