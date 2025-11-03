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

        public override void ShowTimer()
        {
            TimeTextReference.Visible = true;
        }

        public override void OnLevelTimerTimeout()
        {
            TimeLeft--;
            UpdateTimerText();
            if (TimeLeft == 0)
            {
                StopTimer();
                TimerEventBus.Instance.EmitSignal("TimeUp");
                GD.Print("Times up!!!");
            }
            else if (TimeLeft == 100)
            {
                TimerEventBus.Instance.EmitSignal("TimeLow");
                GD.Print("Time low!!!");
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
    }
}