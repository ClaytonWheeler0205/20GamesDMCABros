using Game.Buses;
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
        [Export]
        private NodePath _flagPointsPath;
        private Sprite _flagPointsReference;

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
            _flagPointsReference = GetNode<Sprite>(_flagPointsPath);
        }

        private void StartEndingSequence(Size playerSize)
        {
            _flagAnimation.Play("flag_down");
            _flagSound.Play();
            _vitoVisual.Show();
            _flagPointsReference.Show();
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
            LevelEventBus.Instance.EmitSignal("LevelWalkStarted");
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

        public void OnBodyEntered(Node body)
        {
            if (body is Vito vito)
            {
                if (GlobalPlayerData.PlayerSize == Size.Small)
                {
                    _vitoVisual.GlobalPosition = new Vector2(_vitoVisual.GlobalPosition.x, vito.GlobalPosition.y + 8.0f);
                }
                else
                {
                    _vitoVisual.GlobalPosition = new Vector2(_vitoVisual.GlobalPosition.x, vito.GlobalPosition.y + 16.0f);
                }
                AwardFlagPoints();
                LevelEventBus.Instance.EmitSignal("LevelFinished");
                JinglePlayer.Instance.StopJingle();
                _vitoVisual.Material = vito.Material;
                vito.FreezePlayer();
                vito.Hide();
                StartEndingSequence(GlobalPlayerData.PlayerSize);
            }
        }

        private void AwardFlagPoints()
        {
            if (_vitoVisual.Position.y >= -16.0f)
            {
                PointsEventBus.Instance.EmitSignal("PointsGained", 100);
                _flagPointsReference.Texture = GD.Load<Texture>("res://UI/Art/100Points.png");
            }
            else if (_vitoVisual.Position.y >= -64.0f)
            {
                PointsEventBus.Instance.EmitSignal("PointsGained", 400);
                _flagPointsReference.Texture = GD.Load<Texture>("res://UI/Art/400Points.png");
            }
            else if (_vitoVisual.Position.y >= -80.0f)
            {
                PointsEventBus.Instance.EmitSignal("PointsGained", 800);
                _flagPointsReference.Texture = GD.Load<Texture>("res://UI/Art/800Points.png");
            }
            else if (_vitoVisual.Position.y >= -128.0f)
            {
                PointsEventBus.Instance.EmitSignal("PointsGained", 2000);
                _flagPointsReference.Texture = GD.Load<Texture>("res://UI/Art/2000Points.png");
            }
            else
            {
                PointsEventBus.Instance.EmitSignal("PointsGained", 5000);
            }
        }

        public void OnWalkSequenceFinished(string anim_name)
        {
            LevelEventBus.Instance.EmitSignal("LevelWalkFinished");
        }
    }
}
