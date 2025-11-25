
using Game.Player;

namespace Game.Enemies
{

    public interface Jumpable
    {
        void Squish(Vito jumpingPlayer);
        void AwardJumpingPoints(Vito jumpingPlayer);
    }
}