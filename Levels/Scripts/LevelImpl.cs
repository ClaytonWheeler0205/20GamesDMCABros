using Godot;

namespace Game.Levels
{

    public class LevelImpl : Level
    {
        public override void Start()
        {
            MusicPlayerReference.StartLevelMusic();
        }
    }
}