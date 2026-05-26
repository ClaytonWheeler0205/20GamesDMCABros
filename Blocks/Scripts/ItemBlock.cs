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
        [Export]
        private NodePath _physicalHitboxPath;
        private CollisionShape2D _physicalHitboxReference;
        private Sprite _itemIconReference;
        private Timer _coinsTimer = null;
        private bool _timerStopped = false;

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
                if (_invisible)
                {
                    BlockVisualReference.Visible = false;
                    _physicalHitboxReference.OneWayCollision = true;
                    _physicalHitboxReference.RotationDegrees = 180.0f;
                }

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
            _physicalHitboxReference = GetNode<CollisionShape2D>(_physicalHitboxPath);
        }

        private void CreateCoinsTimer()
        {
            _coinsTimer = new Timer();
            _coinsTimer.OneShot = true;
            _coinsTimer.Connect("timeout", this, nameof(OnCoinsTimerTimerout));
            AddChild(_coinsTimer);
        }

        public override void EnableBlock()
        {
            _hitBlockVisualReference.Visible = false;
            InteractionHitBoxReference.SetDeferred("disabled", false);
            if (_invisible)
            {
                _physicalHitboxReference.SetDeferred("one_way_collision", true);

            }
            else
            {
                BlockVisualReference.Visible = true;
            }
            if (_coinsTimer.IsValid())
                _coinsTimer.Stop();
            _timerStopped = false;
        }

        public void OnBlockHitByPlayer()
        {
            BlockDamageReference.SetDeferred("disabled", false);
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
            if (_timerStopped)
            {
                DisableBlock();
                return;
            }
            StartCoinsTimer();
        }

        private void DisableBlock()
        {
            BlockVisualReference.Visible = false;
            _hitBlockVisualReference.Visible = true;
            InteractionHitBoxReference.SetDeferred("disabled", true);
            _physicalHitboxReference.SetDeferred("one_way_collision", false);
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

        public override void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "bounce")
            {
                CreatePowerup();
                BlockDamageReference.SetDeferred("disabled", true);
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
            _timerStopped = true;
        }
    }
}
