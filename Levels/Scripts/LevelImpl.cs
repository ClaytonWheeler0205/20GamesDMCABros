using Godot;

namespace Game.Levels
{

    public class LevelImpl : Level
    {
        public override void Start()
        {
            MusicPlayerReference.StartLevelMusic();
        }

        public override Vector2 GetPlayerSpawnPoint()
        {
            return StartingPointReference.GlobalPosition;
        }
        public override void ResetEnemies()
        {
            throw new System.NotImplementedException();
        }
    }
}