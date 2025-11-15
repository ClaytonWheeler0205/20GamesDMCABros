using Godot;

namespace Game.Player
{

    public class VitoPaletteComponentImpl : VitoPaletteComponent
    {
        public override void SetPlayerColor(int paletteCode)
        {
            if (paletteCode < 0)
            {
                PlayerMaterial.SetShaderParam("palette_code", (int)CurrentPlayerColor);
                return;
            }
            PlayerMaterial.SetShaderParam("palette_code", paletteCode);
        }

        public override void OnAnimationFinished(string anim_name)
        {
            if (anim_name == "fire_transform")
            {
                PlayerMaterial.SetShaderParam("palette_code", PaletteCode.Fire);
                GetTree().Paused = false;
            }
        }
    }
}