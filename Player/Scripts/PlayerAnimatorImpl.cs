using Godot;

namespace Game.Player
{

    public class PlayerAnimatorImpl : AnimatedSprite, PlayerAnimator
    {
        public Vito PlayerToAnimate { get; set; }
        public MovementComponent PlayerMovement { get; set; }
        [Export]
        private float _minimumSpeedForMovement = 10.0f;

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
                if (Mathf.Abs(playerHorizontalSpeed) < _minimumSpeedForMovement)
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
                        SpeedScale = 1.5f;
                    }
                    else
                    {
                        SpeedScale = 1.0f;
                    }
                    animationToPlay = "walk";
                }
            }
            else
            {
                animationToPlay = "jump";
            }
            Play(animationToPlay);
        }

        private void FlipToCurrentDirection()
        {
            float horizontalDirection = PlayerToAnimate.GetVelocityVector().x;
            if (horizontalDirection > 0.0f)
            {
                FlipH = false;
            }
            else if (horizontalDirection < 0.0f)
            {
                FlipH = true;
            }
        }

        public void ToggleAnimation()
        {
            Visible = !Visible;
            if (!Visible)
            {
                Stop();
            }
        }
    }
}