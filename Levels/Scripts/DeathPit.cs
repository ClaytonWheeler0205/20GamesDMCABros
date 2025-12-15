using Game.Player;
using Godot;

namespace Game.Levels
{

    public class DeathPit : Area2D
    {
        public void OnBodyEntered(Node body)
        {
            if (body is Vito vito)
            {
                vito.Fall();
            }
        }
    }
}