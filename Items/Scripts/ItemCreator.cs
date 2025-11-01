using Game.Player;
using Godot;

namespace Game.Items
{

    public enum Item
    {
        Coin,
        Coins,
        Powerup,
        Life,
        Star
    }
    public static class ItemCreator
    {
        public static Node2D CreateItem(Item itemType)
        {
            PackedScene itemScene = null;
            switch (itemType)
            {
                case Item.Coin:
                    itemScene = GD.Load<PackedScene>("res://Items/Scenes/BlockCoin.tscn");
                    break;
                case Item.Coins:
                    itemScene = GD.Load<PackedScene>("res://Items/Scenes/BlockCoin.tscn");
                    break;
                case Item.Powerup:
                    itemScene = CreatePowerup();
                    break;
                case Item.Life:
                    itemScene = GD.Load<PackedScene>("res://Items/Scenes/LifeShroom.tscn");
                    break;
                case Item.Star:
                    itemScene = GD.Load<PackedScene>("res://Items/Scenes/Star.tscn");
                    break;
            }
            Node2D itemToCreate = itemScene.Instance<Node2D>();
            return itemToCreate;
        }

        private static PackedScene CreatePowerup()
        {
            if (GlobalPlayerData.PlayerSize == Size.Small)
            {
                return GD.Load<PackedScene>("res://Items/Scenes/Mushroom.tscn");
            }
            else
            {
                return GD.Load<PackedScene>("res://Items/Scenes/FireFlower.tscn");
            }
        }
    }
}