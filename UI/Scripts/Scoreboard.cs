using Godot;

namespace Game.UI
{

    public class Scoreboard : Node
    {
        [Export]
        private NodePath _timeUIPath;
        private TimeManager _timeUIReference;
        public TimeManager TimeUIReference
        {
            get { return _timeUIReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _timeUIReference = GetNode<TimeManager>(_timeUIPath);
        }
    }
}