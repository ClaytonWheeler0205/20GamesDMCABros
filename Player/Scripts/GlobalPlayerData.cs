using Godot;

namespace Game.Player
{
    
    public enum Size
    {
        Small,
        Big
    }

    public static class GlobalPlayerData
    {
        public static Size PlayerSize = Size.Small;
    }
}