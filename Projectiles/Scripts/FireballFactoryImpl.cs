namespace Game.Projectiles
{

    public class FireballFactoryImpl : FireballFactory
    {
        public override Fireball GetFireball()
        {
            Fireball fireballToGive = null;
            foreach (Fireball fireball in Fireballs)
            {
                if (fireball.Enabled)
                {
                    continue;
                }
                fireballToGive = fireball;
            }
            return fireballToGive;
        }
    }
}