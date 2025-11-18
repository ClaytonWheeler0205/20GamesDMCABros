using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public abstract class VitoPaletteComponent : Node
    {
        protected enum PaletteCode
        {
            Default = 0,
            Fire = 1,
            Ice = 2
        }
        private PaletteCode _currentPlayerColor = PaletteCode.Default;
        protected PaletteCode CurrentPlayerColor
        {
            get { return _currentPlayerColor; }
            set { _currentPlayerColor = value; }
        }
        private ShaderMaterial _playerMaterial;
        public ShaderMaterial PlayerMaterial
        {
            protected get { return _playerMaterial; }
            set
            {
                if (value.IsValid())
                {
                    _playerMaterial = value;
                }
            }
        }
        private AnimationPlayer _paletteAnimator;
        public AnimationPlayer PaletteAnimator
        {
            protected get { return _paletteAnimator; }
            set
            {
                if (!value.IsValid())
                {
                    return;
                }
                _paletteAnimator = value;
            }
        }
        private bool _isInvincibilityTimerRunning = false;
        public bool IsInvincibilityTimerRunning
        {
            protected get { return _isInvincibilityTimerRunning; }
            set
            {
                _isInvincibilityTimerRunning = value;
            }
        }
        private bool _isFastFlashTimerRunning = false;
        public bool IsFastFlashTimerRunning
        {
            protected get { return _isFastFlashTimerRunning; }
            set
            {
                _isFastFlashTimerRunning = value;
            }
        }

        public abstract void SetPlayerColor(int paletteCode);
        public abstract void OnAnimationFinished(string anim_name);
    }
}