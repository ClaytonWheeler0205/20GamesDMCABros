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
        protected const float SECOND_DUIRATION = 0.4f;

        public override void _Ready()
        {
            SetNodeReferences();
            StartTimer();
        }

        private void SetNodeReferences()
        {
            _levelTimerReference = GetNode<Timer>(_levelTimerPath);
            _timeTextReference = GetNode<Label>(_timeTextPath);
        }

        public abstract void StartTimer();

        public abstract void StopTimer();

        public abstract void HideTimer();

        public abstract void ShowTimer();

        public abstract void OnLevelTimerTimeout();
    }
}