using Godot;

public interface Burnable
{
    CollisionShape2D EnemyHitbox { get; }
    void Burn();
}
