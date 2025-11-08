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

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _topPartToAnimateReference = GetNode<AnimatedSprite>(_topPartToAnimatePath);
            _bottomPartToAnimateReference = GetNode<AnimatedSprite>(_bottomPartToAnimatePath);
        }

        public override void _Process(float delta)
        {
            if (Visible)
            {
                AnimatePlayer();
            }
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
            if (!Visible)
            {
                _topPartToAnimateReference.Stop();
                _bottomPartToAnimateReference.Stop();
            }
        }
    }
}