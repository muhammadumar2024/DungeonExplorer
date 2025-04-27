namespace DungeonExplorer
{
    // Interface for things that can take damage
    public interface IDamageable
    {
        int Health { get; set; } // Allow getting and setting health
        void TakeDamage(int damage);
    }

    // Interface for things that can be picked up
    public interface ICollectible
    {
        string Name { get; } // Items should have names
        void Collect(Player player);
    }

}