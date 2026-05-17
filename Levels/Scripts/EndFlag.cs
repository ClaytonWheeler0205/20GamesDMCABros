using Game.Player;
using Godot;

namespace Game.Levels
{

    public class EndFlag : Node2D
    {
        [Export]
        private NodePath _flagAnimationPath;
        private AnimationPlayer _flagAnimation;
        private bool _flagAnimationFinished;
        [Export]
        private NodePath _vitoPath;
        private AnimatedSprite _vitoVisual;
        private Vector2 _vitoGoalPosition = new Vector2(-8.0f, 0.0f);
        private bool _shouldMoveDown;
        [Export]
        private NodePath _vitoWalkPath;
        private AnimationPlayer _vitoWalk;
        [Export]
        private NodePath _flagSoundPath;
        private AudioStreamPlayer _flagSound;

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _flagAnimation = GetNode<AnimationPlayer>(_flagAnimationPath);
            _vitoVisual = GetNode<AnimatedSprite>(_vitoPath);
            _flagSound = GetNode<AudioStreamPlayer>(_flagSoundPath);
            _vitoWalk = GetNode<AnimationPlayer>(_vitoWalkPath);
        }

        // This is a function for testing only!!!
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("jump"))
                StartEndingSequence(Size.Big);
        }

        private void StartEndingSequence(Size playerSize)
        {
            _flagAnimation.Play("flag_down");
            _flagSound.Play();
            _vitoVisual.Show();
            _shouldMoveDown = true;
            if (playerSize == Size.Big)
            {
                _vitoVisual.Offset = new Vector2(0.0f, -16.0f);
                _vitoVisual.Play("super_climb");
                return;
            }
            _vitoVisual.Play("climb");
        }

        public override void _Process(float delta)
        {
            if (!_shouldMoveDown)
                return;
            _vitoVisual.Position = _vitoVisual.Position.MoveToward(_vitoGoalPosition, 131.0f * delta);
            if (Mathf.IsEqualApprox(_vitoVisual.Position.y, _vitoGoalPosition.y))
            {
                _shouldMoveDown = false;
                _vitoVisual.Stop();
                _vitoVisual.Frame = 0;
                if (_flagAnimationFinished)
                    PlayWalkSequence();
            }

        }

        private async void PlayWalkSequence()
        {
            _vitoVisual.FlipH = true;
            _vitoVisual.Position = new Vector2(-_vitoVisual.Position.x, _vitoVisual.Position.y);
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            _vitoVisual.FlipH = false;
            if (_vitoVisual.Animation == "climb")
                _vitoVisual.Play("walk");
            else
                _vitoVisual.Play("super_walk");
            _vitoWalk.Play("end_walk");
            JinglePlayer.Instance.PlayJingle(JingleType.CourseClear);
        }

        public void OnFlagAnimationFinished(string anim_name)
        {
            _flagAnimationFinished = true;
            if (!_shouldMoveDown)
                PlayWalkSequence();
        }
    }
}
