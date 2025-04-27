namespace DungeonExplorer
{
    // Abstract base class for all living entities in the game
    public abstract class Creature : IDamageable
    {
        public string Name { get; set; }
        public int Health { get; set; }

        // Constructor to ensure name and health are set
        protected Creature(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public abstract void TakeDamage(int damage);
        public bool IsAlive()
        {
            return Health > 0;
        }
    }
}