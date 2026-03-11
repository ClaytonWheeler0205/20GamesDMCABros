using Godot;

namespace Game.Levels
{
    public class CameraBlocker : Node
    {
        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                GetNode<Sprite>("Icon").Visible = false;
            }
        }
    }
}