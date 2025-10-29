using Godot;
using System;
using Util.ExtensionMethods;


namespace Game.Blocks
{

    public enum Item
    {
        Coin,
        Coins,
        Powerup,
        Life,
        Star
    }

    [Tool]
    public class ItemBlock : Block
    {
        private Item _itemInBlock;
        [Export]
        private Item ItemInBlock
        {
            get { return _itemInBlock; }
            set
            {
                _itemInBlock = value;
                SetItemIcon(value);
            }
        }
        [Export]
        private bool _invisible;
        [Export]
        private NodePath _hitBlockVisualPath;
        private Sprite _hitBlockVisualReference;
        [Export]
        private NodePath _hitBlockSoundPath;
        private AudioStreamPlayer _hitBlockSoundReference;
        private Sprite _itemIconReference;

        private void SetItemIcon(Item item)
        {
            Texture itemIcon = null;
            switch (item)
            {
                case Item.Coin:
                    itemIcon = GD.Load<Texture>("res://Items/Art/CoinIcon.png");
                    break;
                case Item.Coins:
                    itemIcon = GD.Load<Texture>("res://Items/Art/CoinsIcon.png");
                    break;
                case Item.Powerup:
                    itemIcon = GD.Load<Texture>("res://Items/Art/Mushroom.png");
                    break;
                case Item.Life:
                    itemIcon = GD.Load<Texture>("res://Items/Art/LifeShroom.png");
                    break;
                case Item.Star:
                    itemIcon = GD.Load<Texture>("res://Items/Art/StarIcon.png");
                    break;
            }
            if (_itemIconReference.IsValid())
            {
                _itemIconReference.Texture = itemIcon;
            }
        }

        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                base._Ready();
                SetNodeReferences();
                BlockVisualReference.Visible = !_invisible;
                if (BlockVisualReference is AnimatedSprite animatedSprite)
                {
                    animatedSprite.Play();
                }
            }
            else
            {
                _itemIconReference = new Sprite();
                _itemIconReference.Modulate = new Color("96ffffff");
                AddChild(_itemIconReference);
                SetItemIcon(ItemInBlock);
            }
        }

        private void SetNodeReferences()
        {
            _hitBlockVisualReference = GetNode<Sprite>(_hitBlockVisualPath);
            _hitBlockSoundReference = GetNode<AudioStreamPlayer>(_hitBlockSoundPath);
        }

        public void OnBlockHitByPlayer()
        {
            BlockVisualReference.Visible = false;
            _hitBlockVisualReference.Visible = true;
            InteractionHitBoxReference.SetDeferred("disabled", true);
            BounceAnimationReference.Play("bounce");
            _hitBlockSoundReference.Play();
        }

        public void OnBounceAnimationFinished(string anim_name)
        {
            if (anim_name == "bounce")
            {
                GD.Print("Create item");
            }
        }
    }
}