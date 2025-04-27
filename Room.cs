using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class Room
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Item> Items { get; private set; } = new List<Item>();
        public List<Monster> Monsters { get; private set; } = new List<Monster>();

        public Room(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Room name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Room description cannot be empty.", nameof(description));

            Name = name;
            Description = description;
        }

        public void AddItem(Item item)
        {
            if (item != null) Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            if (item != null) Items.Remove(item);
        }

        public void AddMonster(Monster monster)
        {
            if (monster != null) Monsters.Add(monster);
        }

        public void RemoveMonster(Monster monster)
        {
            if (monster != null) Monsters.Remove(monster);
        }

        public void DisplayItems()
        {
            if (Items.Any())
            {
                Console.WriteLine(" You see the following items:");
                foreach (var item in Items)
                {
                    Console.WriteLine($"  - {item.Name}");
                }
            }
        }

        public void DisplayMonsters()
        {
            var livingMonsters = Monsters.Where(m => m.IsAlive()).ToList();
            if (livingMonsters.Any())
            {
                Console.WriteLine(" DANGER! You encounter:");
                foreach (var monster in livingMonsters)
                {
                    Console.WriteLine($"  - {monster.Name} (Health: {monster.Health})");
                }
            }
        }
    }
}