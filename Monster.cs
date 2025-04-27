using System;

namespace DungeonExplorer
{
    public abstract class Monster : Creature
    {
        public int AttackDamage { get; protected set; }

        protected Monster(string name, int health, int attackDamage)
            : base(name, health) // Call Creature constructor
        {
            AttackDamage = attackDamage;
        }

        public virtual void Attack(IDamageable target)
        {
            if (target.Health <= 0) return;

            Console.WriteLine($"{Name} attacks {target} for {AttackDamage} damage!");
            target.TakeDamage(AttackDamage);
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"{Name} takes {damage} damage! (Health: {Math.Max(0, Health)})");

            if (!IsAlive())
                Console.WriteLine($"{Name} collapses!");
        }
    }

    public class Goblin : Monster
    {
        public Goblin(string name = "Goblin", int health = 30, int attackDamage = 5)
            : base(name, health, attackDamage) // Pass values to Monster(name, health, attackDamage)
        {
        }
    }


    public class Dragon : Monster
    {
        public Dragon() : base("Ancient Dragon", 150, 25) { }

        public override void Attack(IDamageable target)
        {
            if (target.Health <= 0) return;

            Console.WriteLine($"🔥 {Name} breathes scorching fire at {target} for {AttackDamage} damage!");
            target.TakeDamage(AttackDamage);
        }
    }
}