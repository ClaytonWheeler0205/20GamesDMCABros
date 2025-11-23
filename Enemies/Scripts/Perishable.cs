namespace Game.Enemies
{
    public interface Perishable
    {
        int PerishPoints { get; }
        void Perish();
    }
}