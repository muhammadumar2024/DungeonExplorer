using System;
using System.Linq;

namespace DungeonExplorer
{
    public class Player : Creature
    {
        public Inventory Inventory { get; private set; }
        public int MaxHealth { get; private set; }

        public Player(string name)
            : base(name, 100)
        {
            Inventory = new Inventory();
            MaxHealth = 100;
        }

        public override void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            Health -= damage;
            Console.WriteLine($"OUCH! {Name} took {damage} damage.");

            if (!IsAlive())
            {
                Health = 0;
                Console.WriteLine($"*** {Name} has fallen! Game Over. ***");
            }
            else
            {
                Console.WriteLine($"{Name}'s health is now {Health}/{MaxHealth}.");
            }
        }

        public void Heal(int amount)
        {
            if (amount <= 0) return;

            int effectiveHeal = Math.Min(amount, MaxHealth - Health);

            if (effectiveHeal <= 0)
            {
                Console.WriteLine($"{Name} is already at full health.");
                return;
            }

            Health += effectiveHeal;
            Console.WriteLine($"Ahhh... {Name} healed for {effectiveHeal} health. Current health: {Health}/{MaxHealth}.");
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"\n-- Player Status --\n Name: {Name}\n Health: {Health}/{MaxHealth}");
        }
    }
}