using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class Inventory
    {
        private List<Item> items = new List<Item>();

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public List<Item> GetItems()
        {
            return new List<Item>(items); // Defensive copy
        }

        public List<Weapon> GetWeapons()
        {
            return items.OfType<Weapon>().ToList();
        }

        public List<Potion> GetPotionsSortedByHealing()
        {
            return items.OfType<Potion>()
                        .OrderByDescending(p => p.Healing)
                        .ToList();
        }

        public void DisplayInventory()
        {
            Console.WriteLine("\n-- Inventory --");
            if (!items.Any())
            {
                Console.WriteLine(" Your backpack is empty.");
            }
            else
            {
                var groupedItems = items
                    .GroupBy(item => item.Name)
                    .Select(group => new { Name = group.Key, Count = group.Count(), Item = group.First() });

                foreach (var group in groupedItems)
                {
                    Console.WriteLine($" - {group.Name} (x{group.Count}): {group.Item.Description}");
                }
            }
            Console.WriteLine("--------------");
        }
    }
}