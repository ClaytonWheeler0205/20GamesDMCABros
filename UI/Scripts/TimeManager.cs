using Game.Buses;
using Godot;

namespace Game.UI
{

    public abstract class TimeManager : Node
    {
        [Export]
        private NodePath _levelTimerPath;
        private Timer _levelTimerReference;
        protected Timer LevelTimerReference
        {
            get { return _levelTimerReference; }
        }
        [Export]
        private NodePath _timeTextPath;
        private Label _timeTextReference;
        protected Label TimeTextReference
        {
            get { return _timeTextReference; }
        }
        [Export]
        private NodePath _timePointSoundPath;
        private AudioStreamPlayer _timePointSoundReference;
        protected AudioStreamPlayer TimePointSoundReference
        {
            get { return _timePointSoundReference; }
        }
        [Export]
        private NodePath _timePointAnimationPath;
        private AnimationPlayer _timePointAnimationReference;
        protected AnimationPlayer TimePointAnimationReference
        {
            get { return _timePointAnimationReference; }
        }
        private int _timeLeft = 400;
        public int TimeLeft
        {
            protected get { return _timeLeft; }
            set
            {
                if (value >= 0)
                {
                    _timeLeft = value;
                }
            }
        }
        private int _timeOnesPlace;
        protected int TimeOnesPlace
        {
            get { return _timeOnesPlace; }
            set { _timeOnesPlace = value; }
        }
        protected const float SECOND_DUIRATION = 0.4f;

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
            StartTimer();
        }

        private void SetNodeConnections()
        {
            PlayerEventBus.Instance.Connect("PipeEntered", this, nameof(OnPipeEntered));
            LevelEventBus.Instance.Connect("PipeTransitionFinished", this, nameof(OnPipeTransitionFinished));
            PlayerEventBus.Instance.Connect("PipeExitAnimationFinished", this, nameof(OnPipeExitAnimationFinished));
            LevelEventBus.Instance.Connect("LevelFinished", this, nameof(StopTimer));
            LevelEventBus.Instance.Connect("LevelWalkFinished", this, nameof(OnLevelWalkFinished));
        }

        private void SetNodeReferences()
        {
            _levelTimerReference = GetNode<Timer>(_levelTimerPath);
            _timeTextReference = GetNode<Label>(_timeTextPath);
            _timePointSoundReference = GetNode<AudioStreamPlayer>(_timePointSoundPath);
            _timePointAnimationReference = GetNode<AnimationPlayer>(_timePointAnimationPath);
        }

        public abstract void StartTimer();

        public abstract void StopTimer();
        public abstract void ResetTimer();

        public abstract void HideTimer();

        public abstract void ShowTimer();
        public abstract void GiveTimePoint();

        public abstract void OnLevelTimerTimeout();
        public abstract void OnPipeEntered();
        public abstract void OnPipeTransitionFinished(bool playExitAnimation);
        public abstract void OnPipeExitAnimationFinished();
        public abstract void OnLevelWalkFinished();
    }
}
