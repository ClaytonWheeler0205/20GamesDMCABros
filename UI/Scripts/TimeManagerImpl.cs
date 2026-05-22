using Game.Buses;
using Godot;

namespace Game.UI
{

    public class TimeManagerImpl : TimeManager
    {
        public override void StartTimer()
        {
            LevelTimerReference.Start(SECOND_DUIRATION);
        }

        public override void StopTimer()
        {
            LevelTimerReference.Stop();
        }

        public override void HideTimer()
        {
            TimeTextReference.Visible = false;
        }

        public override void ResetTimer()
        {
            TimeLeft = 400;
            UpdateTimerText();
        }

        public override void ShowTimer()
        {
            TimeTextReference.Visible = true;
        }

        public override void GiveTimePoint()
        {
            TimePointSoundReference.Play();
            TimeLeft--;
            UpdateTimerText();
            PointsEventBus.Instance.EmitSignal("PointsGained", 50);
            if (TimeLeft == 0)
            {
                TimePointAnimationReference.Stop();
                LevelEventBus.Instance.EmitSignal("FinalScoreCountFinished");
            }
        }

        public override void OnLevelTimerTimeout()
        {
            TimeLeft--;
            UpdateTimerText();
            if (TimeLeft == 0)
            {
                StopTimer();
                TimerEventBus.Instance.EmitSignal("TimeUp");
            }
            else if (TimeLeft == 100)
            {
                TimerEventBus.Instance.EmitSignal("TimeLow");
            }
        }

        private void UpdateTimerText()
        {
            if (TimeLeft >= 100)
            {
                TimeTextReference.Text = $"{TimeLeft}";
            }
            else if (TimeLeft >= 10)
            {
                TimeTextReference.Text = $"0{TimeLeft}";
            }
            else
            {
                TimeTextReference.Text = $"00{TimeLeft}";
            }
        }

        public override void OnPipeEntered()
        {
            StopTimer();
        }

        public override void OnPipeTransitionFinished(bool playExitAnimation)
        {
            if (playExitAnimation)
                return;
            StartTimer();
        }

        public override void OnPipeExitAnimationFinished()
        {
            StartTimer();
        }

        public override void OnLevelWalkFinished()
        {
            TimePointAnimationReference.Play("timer_points");
        }
    }
}
