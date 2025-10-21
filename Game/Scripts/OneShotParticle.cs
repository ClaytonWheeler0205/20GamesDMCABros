using Godot;
using Util.ExtensionMethods;

namespace Game
{

    public class OneShotParticle : CPUParticles2D
    {
        [Signal]
        public delegate void ParticleFinished();

        public override void _Ready()
        {
            Emitting = true;
        }

        public override void _Process(float delta)
        {
            if (!Emitting)
            {
                EmitSignal("ParticleFinished");
                this.SafeQueueFree();
            }
        }
    }
}