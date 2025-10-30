using Godot;
using Util.ExtensionMethods;
using Game.Items;


namespace Game.Blocks
{

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
        private Timer _coinsTimer = null;

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
                if (_itemInBlock != Item.Coins)
                {
                    return;
                }
                CreateCoinsTimer();
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

        private void CreateCoinsTimer()
        {
            _coinsTimer = new Timer();
            _coinsTimer.OneShot = true;
            _coinsTimer.Connect("timeout", this, nameof(OnCoinsTimerTimerout));
            AddChild(_coinsTimer);
        }

        public void OnBlockHitByPlayer()
        {
            if (_itemInBlock != Item.Coins)
            {
                DisableBlock();
            }
            BounceAnimationReference.Play("bounce");
            _hitBlockSoundReference.Play();

            if (_itemInBlock != Item.Coin && _itemInBlock != Item.Coins)
            {
                return;
            }
            CreateCoin();
            if (_itemInBlock != Item.Coins)
            {
                return;
            }
            StartCoinsTimer();
        }

        private void DisableBlock()
        {
            BlockVisualReference.Visible = false;
            _hitBlockVisualReference.Visible = true;
            InteractionHitBoxReference.SetDeferred("disabled", true);
        }

        private void CreateCoin()
        {
            Node2D coinNode = ItemCreator.CreateItem(_itemInBlock);
            coinNode.Position = new Vector2(0, -16.0f);
            AddChild(coinNode);
        }

        private void StartCoinsTimer()
        {
            if (_coinsTimer.IsValid() && _coinsTimer.IsStopped())
            {
                _coinsTimer.Start(3.8f);
            }
        }

        public void OnBounceAnimationFinished(string anim_name)
        {
            if (anim_name == "bounce")
            {
                CreatePowerup();
            }
        }

        private void CreatePowerup()
        {
            if (_itemInBlock == Item.Coin || _itemInBlock == Item.Coins)
            {
                return;
            }
            Node powerupNode = ItemCreator.CreateItem(_itemInBlock);
            AddChild(powerupNode);
        }

        public void OnCoinsTimerTimerout()
        {
            DisableBlock();
        }
    }
}