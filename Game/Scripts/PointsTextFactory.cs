using Godot;
using System.Collections.Generic;
using Util.ExtensionMethods;

namespace Game
{

    public class PointsTextFactory : Node
    {
        private static Dictionary<int, Texture> _pointTextures = new Dictionary<int, Texture>()
        {
            {0, GD.Load<Texture>("res://UI/Art/1UP.png")},
            {100, GD.Load<Texture>("res://UI/Art/100Points.png")},
            {200, GD.Load<Texture>("res://UI/Art/200Points.png")},
            {400, GD.Load<Texture>("res://UI/Art/400Points.png")},
            {500, GD.Load<Texture>("res://UI/Art/500Points.png")},
            {800, GD.Load<Texture>("res://UI/Art/800Points.png")},
            {1000, GD.Load<Texture>("res://UI/Art/1000Points.png")},
            {2000, GD.Load<Texture>("res://UI/Art/2000Points.png")},
            {4000, GD.Load<Texture>("res://UI/Art/4000Points.png")},
            {5000, GD.Load<Texture>("res://UI/Art/5000Points.png")},
            {8000, GD.Load<Texture>("res://UI/Art/8000Points.png")}
        };
        private static PackedScene _pointTextScene = GD.Load<PackedScene>("res://Game/Scenes/PointText.tscn");
        private static PackedScene _lifeSoundScene = GD.Load<PackedScene>("res://Game/Scenes/OneShotAudio.tscn");
        private static AudioStream _lifeSoundStream = GD.Load<AudioStream>("res://Items/Audio/life_get.wav");
        private static Node _instance;

        public override void _Ready()
        {
            if (_instance != null && _instance != this)
            {
                this.SafeQueueFree();
                return;
            }
            _instance = this;
        }


        public static void CreatePointTextFromEnemy(int pointValue, Vector2 enemyPosition)
        {
            PointText _pointText = _pointTextScene.Instance<PointText>();
            _instance.AddChild(_pointText);
            _pointText.PointVisualReference.Texture = _pointTextures[pointValue];
            _pointText.GlobalPosition = new Vector2(enemyPosition.x, enemyPosition.y - 16.0f);
            if (pointValue == 0)
            {
                OneShotAudio _lifeSound = _lifeSoundScene.Instance<OneShotAudio>();
                _lifeSound.Stream = _lifeSoundStream;
                _instance.AddChild(_lifeSound);
            }
        }
    }
}