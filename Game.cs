using System;
using System.Linq;

namespace DungeonExplorer
{
    class Game
    {
        private Player player;
        private GameMap map;
        private string currentRoomName;
        private Random random = new Random();

        public Game()
        {
            map = new GameMap();
        }

        public void Start()
        {
            Console.WriteLine("Welcome to Dungeon Explorer!");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine() ?? "Adventurer";
            player = new Player(name);
            CreateMap();
            currentRoomName = "Entrance";
            Play();
        }

        private void CreateMap()
        {
            Weapon sword = new Weapon { Name = "Sword", Description = "A basic sword", Damage = 10 };
            Potion healthPotion = new Potion { Name = "Health Potion", Description = "Restores 20 health", Healing = 20 };
            Weapon axe = new Weapon { Name = "Axe", Description = "A rusty axe", Damage = 15 };

            Room entrance = new Room("Entrance", "You are at the dungeon entrance. It's damp and cold.");
            Room hall = new Room("Hall", "A long, dark hallway. You hear dripping water.");
            Room armory = new Room("Armory", "A dusty room filled with old weapon racks.");
            Room treasureRoom = new Room("Treasure Room", "A bright room! You see a pile of glittering gold.");

            entrance.AddItem(sword);
            hall.AddItem(healthPotion);
            armory.AddItem(axe);

            hall.AddMonster(new Goblin());
            armory.AddMonster(new Goblin(name: "Armored Goblin", health: 40, attackDamage: 8));
            treasureRoom.AddMonster(new Dragon());

            map.AddRoom(entrance);
            map.AddRoom(hall);
            map.AddRoom(armory);
            map.AddRoom(treasureRoom);
        }

        private void Play()
        {
            Console.WriteLine($"\n{player.Name}, you cautiously enter the dungeon...");

            while (player.Health > 0)
            {
                Room currentRoom = map.Rooms[currentRoomName];
                Console.WriteLine($"\n--- {currentRoom.Name} ---");
                Console.WriteLine(currentRoom.Description);

                currentRoom.DisplayMonsters();
                currentRoom.DisplayItems();
                player.DisplayStatus();

                if (currentRoom.Monsters.Any())
                {
                    Console.WriteLine("\nMonsters in the room growl menacingly!");
                    foreach (var monster in currentRoom.Monsters.ToList())
                    {
                        if (monster.Health > 0)
                        {
                            monster.Attack(player);
                            if (player.Health <= 0) break;
                        }
                    }
                    if (player.Health <= 0) break;
                }

                Console.Write("\nEnter command (move <room>, look, pickup <item>, inventory, attack <monster>, use <item>, quit): ");
                string input = Console.ReadLine()?.ToLower().Trim() ?? "";
                string[] parts = input.Split(new char[] {' '}, 2, StringSplitOptions.None);
                string command = parts[0];
                string argument = parts.Length > 1 ? parts[1] : null;

                try
                {
                    switch (command)
                    {
                        case "move":
                            MoveRoom(argument);
                            break;
                        case "look":
                            break;
                        case "pickup":
                            PickUpItem(currentRoom, argument);
                            break;
                        case "inventory":
                        case "inv":
                            player.Inventory.DisplayInventory();
                            break;
                        case "attack":
                            AttackMonster(currentRoom, argument);
                            break;
                        case "use":
                            UseItem(argument);
                            break;
                        case "quit":
                            Console.WriteLine("You flee the dungeon. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("⚠️ Unknown command. Try: move <room>, look, pickup <item>, inventory, attack <monster>, use <item>, quit.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ An error occurred: {ex.Message}");
                }

                currentRoom.Monsters.RemoveAll(m => m.Health <= 0);

                if (random.Next(4) == 0)
                {
                    int damage = random.Next(5, 16);
                    Console.WriteLine($"\n⚠️ Clumsy! You stumbled on a loose stone and took {damage} damage.");
                    player.TakeDamage(damage);
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("\nYour adventure ends here...");
            }
        }

        private void MoveRoom(string targetRoomName)
        {
            if (string.IsNullOrWhiteSpace(targetRoomName))
            {
                Console.WriteLine("⚠️ Move where? Specify a room name (e.g., 'move Hall').");
                Console.WriteLine("Available rooms (usually): " + string.Join(", ", map.Rooms.Keys.Where(k => k != currentRoomName)));
                return;
            }

            if (map.Rooms.ContainsKey(targetRoomName) && targetRoomName != currentRoomName)
            {
                currentRoomName = targetRoomName;
                Console.WriteLine($"You move to the {currentRoomName}.");
            }
            else if (targetRoomName == currentRoomName)
            {
                Console.WriteLine("You are already in that room.");
            }
            else
            {
                Console.WriteLine($"⚠️ Cannot find or move to a room named '{targetRoomName}'.");
            }
        }

        private void PickUpItem(Room currentRoom, string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                Console.WriteLine("⚠️ Pick up what? Specify an item name.");
                return;
            }

            Item item = currentRoom.Items
                .FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (item != null)
            {
                item.Collect(player);
                currentRoom.RemoveItem(item);
            }
            else
            {
                Console.WriteLine($"⚠️ There is no '{itemName}' here to pick up.");
            }
        }

        private void AttackMonster(Room currentRoom, string monsterName)
        {
            if (string.IsNullOrWhiteSpace(monsterName))
            {
                Console.WriteLine("⚠️ Attack what? Specify a monster name.");
                return;
            }

            if (!currentRoom.Monsters.Any())
            {
                Console.WriteLine("There are no monsters here to attack.");
                return;
            }

            Monster monster = currentRoom.Monsters
                .FirstOrDefault(m => m.Name.Equals(monsterName, StringComparison.OrdinalIgnoreCase) && m.Health > 0);

            if (monster != null)
            {
                Weapon equippedWeapon = player.Inventory.GetItems()
                                        .OfType<Weapon>()
                                        .OrderByDescending(w => w.Damage)
                                        .FirstOrDefault();

                int attackDamage = 5;
                string weaponText = "with your fists";
                if (equippedWeapon != null)
                {
                    attackDamage = equippedWeapon.Damage;
                    weaponText = $"with your {equippedWeapon.Name}";
                }

                Console.WriteLine($"You attack the {monster.Name} {weaponText} for {attackDamage} damage!");
                monster.TakeDamage(attackDamage);

                if (monster.Health <= 0)
                {
                    Console.WriteLine($"You defeated the {monster.Name}!");
                }
            }
            else
            {
                Console.WriteLine($"⚠️ There is no living monster named '{monsterName}' here.");
            }
        }

        private void UseItem(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                Console.WriteLine("⚠️ Use what? Specify an item name from your inventory.");
                return;
            }

            Item itemToUse = player.Inventory.GetItems()
                .FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (itemToUse == null)
            {
                Console.WriteLine($"⚠️ You don't have '{itemName}' in your inventory.");
                return;
            }

            if (itemToUse is Potion potion)
            {
                if (player.Health >= 100)
                {
                    Console.WriteLine("You are already at full health.");
                    return;
                }
                potion.Use(player);
            }
            else if (itemToUse is Weapon)
            {
                Console.WriteLine($"You examine your {itemToUse.Name}. It's for attacking!");
            }
            else
            {
                Console.WriteLine($"You can't 'use' the {itemToUse.Name} in this way.");
            }
        }
    }
}