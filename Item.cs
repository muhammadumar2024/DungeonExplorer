using System;

namespace DungeonExplorer
{
    public abstract class Item : ICollectible
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public abstract void Collect(Player player);

        public override string ToString() => $"{Name}: {Description}";
    }

    public class Weapon : Item
    {
        public int Damage { get; set; }

        public override void Collect(Player player)
        {
            player.Inventory.AddItem(this);
            Console.WriteLine($"{player.Name} picked up the weapon: {Name} (Damage: {Damage})!");
        }
    }

    public class Potion : Item
    {
        public int Healing { get; set; }

        public override void Collect(Player player)
        {
            player.Inventory.AddItem(this);
            Console.WriteLine($"{player.Name} picked up the potion: {Name} (Heals: {Healing})!");
        }

        public void Use(Player player)
        {
            Console.WriteLine($"{player.Name} uses the {Name}.");
            player.Heal(Healing);
            player.Inventory.RemoveItem(this);
        }
    }
}