
using Game.Player;
using Godot;

namespace Game.Enemies
{

    public interface Jumpable
    {
        AudioStreamPlayer SquishSoundPlayerReference { get; }
        void Squish(Vito jumpingPlayer);
        void AwardJumpingPoints(Vito jumpingPlayer);
    }
}