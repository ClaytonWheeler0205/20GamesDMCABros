using Game.Buses;
using Godot;

namespace Game.UI
{

    public class CoinsManager : Node
    {
        [Export]
        private NodePath _coinCounterTextPath;
        private Label _coinCounterTextReference;
        [Export]
        private NodePath _lifeGainedSoundPath;
        private AudioStreamPlayer _lifeGainedSoundReference;
        private int _coins = 0;

        public override void _Ready()
        {
            SetNodeReferences();
            SetNodeConnections();
        }

        private void SetNodeReferences()
        {
            _coinCounterTextReference = GetNode<Label>(_coinCounterTextPath);
            _lifeGainedSoundReference = GetNode<AudioStreamPlayer>(_lifeGainedSoundPath);
        }

        private void SetNodeConnections()
        {
            CoinEventBus.Instance.Connect("CoinCollected", this, nameof(OnCoinCollected));
        }

        public void OnCoinCollected()
        {
            _coins++;
            if (_coins == 100)
            {
                _coins = 0;
                _lifeGainedSoundReference.Play();
                LivesEventBus.Instance.EmitSignal("LifeGained");
            }
            UpdateCoinText();
        }

        private void UpdateCoinText()
        {
            if (_coins >= 10)
            {
                _coinCounterTextReference.Text = $"x{_coins}";
            }
            else
            {
                _coinCounterTextReference.Text = $"x0{_coins}";
            }
        }
    }
}