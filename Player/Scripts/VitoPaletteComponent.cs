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

        public abstract void SetPlayerColor(int paletteCode);
        public abstract void OnAnimationFinished(string anim_name);
    }
}