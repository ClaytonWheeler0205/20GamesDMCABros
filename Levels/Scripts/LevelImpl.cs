using Game.Blocks;
using Game.Enemies;
using Game.Items;
using Godot;

namespace Game.Levels
{

    public class LevelImpl : Level
    {
        public override void Start(bool firstLoad)
        {
            if (!firstLoad)
            {
                EnableLevelObjects();
            }
            MusicPlayerReference.StartLevelMusic();
        }

        private void EnableLevelObjects()
        {
            foreach (Node node in EnemyContainerReference.GetChildren())
            {
                if (node is Enemy enemy)
                {
                    EnableEnemyIfNecessary(enemy);
                }
            }
            foreach (Node node in BlockContainerReference.GetChildren())
            {
                if (node is Block block)
                {
                    EnableBlockIfNecessary(block);
                }
            }
            foreach (Node node in CoinContainerReference.GetChildren())
            {
                if (node is Coin coin)
                {
                    EnableCoinIfNecessary(coin);
                }
            }
        }

        private void EnableEnemyIfNecessary(Enemy enemyToEnable)
        {
            if (StartingPointReference.GlobalPosition.x < enemyToEnable.GlobalPosition.x)
            {
                enemyToEnable.EnableEnemy();
            }
        }

        private void EnableBlockIfNecessary(Block blockToEnable)
        {
            if (StartingPointReference.GlobalPosition.x < blockToEnable.GlobalPosition.x)
            {
                blockToEnable.EnableBlock();
            }
        }

        private void EnableCoinIfNecessary(Coin coinToEnable)
        {
            if (StartingPointReference.GlobalPosition.x < coinToEnable.GlobalPosition.x)
            {
                coinToEnable.EnableCoin();
            }
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