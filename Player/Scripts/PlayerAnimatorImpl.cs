using Game.Buses;
using Godot;

namespace Game.Player
{

    public class PlayerAnimatorImpl : AnimatedSprite, PlayerAnimator
    {
        public Vito PlayerToAnimate { get; set; }
        public MovementComponent PlayerMovement { get; set; }
        [Export]
        private float _minimumSpeedForMovement = 10.0f;
        private Vector2 _shrinkSpriteOffset = new Vector2(0, -8);
        private bool _isShrinking = false;

        public override void _Ready()
        {
            SetNodeConnections();
        }

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("DamageTaken", this, nameof(OnDamageTaken));
            PlayerEventBus.Instance.Connect("PlayerDied", this, nameof(OnPlayerDied));
            PlayerEventBus.Instance.Connect("PlayerReset", this, nameof(OnPlayerReset));
        }

        public override void _Process(float delta)
        {
            if (!Visible || _isShrinking)
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

        public void OnDamageTaken()
        {
            SpeedScale = 1.0f;
            PauseMode = PauseModeEnum.Process;
            _isShrinking = true;
            Offset = _shrinkSpriteOffset;
            Play("shrink");
            FlipToCurrentDirection();
            GetTree().Paused = true;
        }

        public void  OnAnimationFinished()
        {
            if (Animation != "shrink")
            {
                return;
            }
            CleanupShrinkAnimation();
        }

        private void CleanupShrinkAnimation()
        {
            _isShrinking = false;
            Offset = Vector2.Zero;
            PauseMode = PauseModeEnum.Stop;
            GetTree().Paused = false;
        }

        public void OnPlayerDied()
        {
            Visible = true;
            Play("death");
            GetTree().Paused = true;
        }

        public void OnPlayerReset()
        {
            Play("idle");
            FlipH = false;
        }
    }
}