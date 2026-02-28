using Godot;
using Util.ExtensionMethods;

namespace Game.Player
{

    public abstract class StarComponent : Node
    {
        [Export]
        private NodePath _invincibilityTimePath;
        private Timer _invincibilityTimeReference;
        protected Timer InvincibilityTimeReference
        {
            get { return _invincibilityTimeReference; }
        }
        private float _invincibilityTime;
        protected float InvincibilityTime
        {
            get { return _invincibilityTime; }
        }
        [Export]
        private NodePath _musicTimePath;
        private Timer _musicTimeReference;
        protected Timer MusicTimeReference
        {
            get { return _musicTimeReference; }
        }
        private float _musicTime;
        protected float MusicTime
        {
            get { return _musicTime; }
        }
        [Export]
        private NodePath _fastFlashTimePath;
        private Timer _fastFlashTimeReference;
        protected Timer FastFlashTimeReference
        {
            get { return _fastFlashTimeReference; }
        }
        private float _fastFlashTime;
        protected float FastFlashTime
        {
            get { return _fastFlashTime; }
        }
        private AnimationPlayer _paletteAnimator;
        public AnimationPlayer PaletteAnimatior
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
        private VitoPaletteComponent _playerPalette;
        public VitoPaletteComponent PlayerPalette
        {
            protected get { return _playerPalette; }
            set
            {
                if (!value.IsValid())
                {
                    return;
                }
                _playerPalette = value;
            }
        }

        public override void _Ready()
        {
            SetNodeReferences();
            _invincibilityTime = _invincibilityTimeReference.WaitTime;
            _musicTime = _musicTimeReference.WaitTime;
            _fastFlashTime = _fastFlashTimeReference.WaitTime;
        }

        public override void _Process(float delta)
        {
            _playerPalette.IsInvincibilityTimerRunning = !_invincibilityTimeReference.IsStopped();
            _playerPalette.IsFastFlashTimerRunning = !_fastFlashTimeReference.IsStopped();
        }


        private void SetNodeReferences()
        {
            _invincibilityTimeReference = GetNode<Timer>(_invincibilityTimePath);
            _musicTimeReference = GetNode<Timer>(_musicTimePath);
            _fastFlashTimeReference = GetNode<Timer>(_fastFlashTimePath);
        }

        public abstract void StartTimers();
        public abstract void ForceStopinvincibility();
        public abstract void OnInvincibilityTimeTimeout();
        public abstract void OnMusicTimeTimeout();
        public abstract void OnFastFlashTimeTimeout();
    }
}