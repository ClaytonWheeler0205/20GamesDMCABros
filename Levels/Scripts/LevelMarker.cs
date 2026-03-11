using Godot;

namespace Game.Levels
{

    [Tool]
    public class LevelMarker : Node2D
    {
        private Sprite _markerIconReference;
        public Sprite MarkerIconReference
        {
            get { return _markerIconReference; }
        }

        public override void _Ready()
        {
            SetNodeReferences();
        }

        private void SetNodeReferences()
        {
            _markerIconReference = GetChild<Sprite>(0);
        }
    }
}