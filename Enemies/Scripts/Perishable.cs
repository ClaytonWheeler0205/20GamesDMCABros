using Godot;

namespace Game.Enemies
{
    public interface Perishable
    {
        AudioStreamPlayer DeathSoundPlayerReference { get; }
        int PerishPoints { get; }
        void Perish();
    }
}