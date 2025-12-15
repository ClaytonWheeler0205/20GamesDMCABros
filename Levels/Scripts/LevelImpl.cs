using Game.Enemies;
using Godot;

namespace Game.Levels
{

    public class LevelImpl : Level
    {
        public override void Start()
        {
            foreach (Node node in EnemyContainerReference.GetChildren())
            {
                if (node is Enemy enemy)
                {
                    enemy.EnableEnemy();
                }
            }
            MusicPlayerReference.StartLevelMusic();
        }

        public override Vector2 GetPlayerSpawnPoint()
        {
            return StartingPointReference.GlobalPosition;
        }

        public override void ResetLevel()
        {
            ResetEnemies();
        }

        private void ResetEnemies()
        {
            foreach (Node node in EnemyContainerReference.GetChildren())
            {
                if (node is Enemy enemy)
                {
                    enemy.DisableEnemy();
                    enemy.GlobalPosition = enemy.StartingPosition;
                }
            }
        }
    }
}