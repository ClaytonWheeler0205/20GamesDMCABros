using Game.Blocks;
using Game.Buses;
using Game.Enemies;
using Game.Items;
using Godot;
using Util;
using Util.ExtensionMethods;

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
                return;
            }
            enemyToEnable.SafeQueueFree();
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

        public override void ResetPlayerSpawnPoint()
        {
            StartingPointReference.GlobalPosition = LevelStartPosition;
            CameraPointPosition = CameraStartPosition;
            LastCheckpointInSubworld = false;
        }

        public override void ResetLevel()
        {
            ResetEnemies();
            MusicPlayerReference.IsLowTime = false;
            InSubworld = LastCheckpointInSubworld;
            MusicPlayerReference.InSubworld = LastCheckpointInSubworld;
            UpdateLevelPalette();
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

        public override void OnPlayerReachedCheckpoint(Vector2 checkpointPosition, Vector2 cameraPosition, bool lastCheckpointInSubworld)
        {
            StartingPointReference.GlobalPosition = checkpointPosition;
            CameraPointPosition = cameraPosition;
            LastCheckpointInSubworld = lastCheckpointInSubworld;
        }

        public override void OnPipeTransitionFinished(bool playExitAnimation)
        {
            InSubworld = !InSubworld;
            UpdateLevelPalette();
        }

        private void UpdateLevelPalette()
        {
            if (InSubworld)
            {
                PaletteMaterial.SetShaderParam("palette_code", (int)SubworldType);
                CoinsMaterial.SetShaderParam("palette_code", (int)SubworldType);
            }
            else
            {
                PaletteMaterial.SetShaderParam("palette_code", (int)WorldType);
                CoinsMaterial.SetShaderParam("palette_code", (int)WorldType);
            }
        }

        public override void OnFinalScoreCountFinished(int secondForFireworks)
        {
            FlagAnimationReference.Play("flag_up");
            SecondOnesPlace = secondForFireworks;
        }

        public override void OnFlagAnimationFinished(string anim_name)
        {
            if (anim_name != "flag_up")
                return;
            if (SecondOnesPlace != 1 && SecondOnesPlace != 3 && SecondOnesPlace != 6)
            {
                EmitSignal("FireworksFinished");
                return;
            }
            PlayFirework();
        }

        private void PlayFirework()
        {
            if (SecondOnesPlace == 0)
            {
                EmitSignal("FireworksFinished");
                return;
            }
            while (FireworkLocationIndex == LastFireWorkLocationIndex)
                FireworkLocationIndex = GDRandom.RandiRange(0, FireworkLocations.Count - 1);
            FireworkReference.GlobalPosition = FireworkLocations[FireworkLocationIndex].GlobalPosition;
            LastFireWorkLocationIndex = FireworkLocationIndex;
            FireworkReference.Frame = 0;
            FireworkReference.Show();
            FireworkReference.Play("firework");
            FireworkExplosionReference.Play();
            PointsEventBus.Instance.EmitSignal("PointsGained", 500);
            SecondOnesPlace--;
        }

        public async override void OnFireworkAnimationFinished()
        {
            FireworkReference.Hide();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            PlayFirework();
        }
    }
}
