using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonExplorer;

namespace DungeonExplorer.Tests
{
    [TestClass]
    public class UnitTests
    {
        #region Player Tests

        [TestMethod]
        public void Player_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var player = new Player("TestPlayer");

            // Assert
            Assert.AreEqual("TestPlayer", player.Name);
            Assert.AreEqual(100, player.Health);
            Assert.AreEqual(100, player.MaxHealth);
            Assert.IsNotNull(player.Inventory);
            Assert.IsTrue(player.IsAlive());
        }

        [TestMethod]
        public void Player_TakeDamage_ReducesHealth()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act
            player.TakeDamage(25);
            
            // Assert
            Assert.AreEqual(75, player.Health);
            Assert.IsTrue(player.IsAlive());
        }

        [TestMethod]
        public void Player_TakeDamage_ZeroOrNegative_NoChange()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act
            player.TakeDamage(0);
            player.TakeDamage(-5);
            
            // Assert
            Assert.AreEqual(100, player.Health);
        }

        [TestMethod]
        public void Player_TakeDamage_ExceedsHealth_SetsToZero()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act
            player.TakeDamage(150);
            
            // Assert
            Assert.AreEqual(0, player.Health);
            Assert.IsFalse(player.IsAlive());
        }

        [TestMethod]
        public void Player_Heal_IncreasesHealth()
        {
            // Arrange
            var player = new Player("TestPlayer");
            player.TakeDamage(30);
            
            // Act
            player.Heal(15);
            
            // Assert
            Assert.AreEqual(85, player.Health);
        }

        [TestMethod]
        public void Player_Heal_CannotExceedMaxHealth()
        {
            // Arrange
            var player = new Player("TestPlayer");
            player.TakeDamage(10);
            
            // Act
            player.Heal(20);
            
            // Assert
            Assert.AreEqual(100, player.Health);
        }

        [TestMethod]
        public void Player_Heal_AtFullHealth_NoChange()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act
            player.Heal(10);
            
            // Assert
            Assert.AreEqual(100, player.Health);
        }

        [TestMethod]
        public void Player_Heal_ZeroOrNegative_NoChange()
        {
            // Arrange
            var player = new Player("TestPlayer");
            player.TakeDamage(20);
            
            // Act
            player.Heal(0);
            player.Heal(-5);
            
            // Assert
            Assert.AreEqual(80, player.Health);
        }

        [TestMethod]
        public void Player_IsAlive_ReturnsCorrectValue()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act & Assert
            Assert.IsTrue(player.IsAlive());
            
            player.TakeDamage(50);
            Assert.IsTrue(player.IsAlive());
            
            player.TakeDamage(50);
            Assert.IsFalse(player.IsAlive());
        }

        #endregion

        #region Monster Tests

        [TestMethod]
        public void Goblin_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var goblin = new Goblin();
            
            // Assert
            Assert.AreEqual("Goblin", goblin.Name);
            Assert.AreEqual(30, goblin.Health);
            Assert.AreEqual(5, goblin.AttackDamage);
            Assert.IsTrue(goblin.IsAlive());
        }

        [TestMethod]
        public void Goblin_CustomConstructor_InitializesCorrectly()
        {
            // Arrange & Act
            var goblin = new Goblin("Elite Goblin", 50, 10);
            
            // Assert
            Assert.AreEqual("Elite Goblin", goblin.Name);
            Assert.AreEqual(50, goblin.Health);
            Assert.AreEqual(10, goblin.AttackDamage);
        }

        [TestMethod]
        public void Dragon_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var dragon = new Dragon();
            
            // Assert
            Assert.AreEqual("Ancient Dragon", dragon.Name);
            Assert.AreEqual(150, dragon.Health);
            Assert.AreEqual(25, dragon.AttackDamage);
            Assert.IsTrue(dragon.IsAlive());
        }

        [TestMethod]
        public void Monster_TakeDamage_ReducesHealth()
        {
            // Arrange
            var goblin = new Goblin();
            
            // Act
            goblin.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(20, goblin.Health);
            Assert.IsTrue(goblin.IsAlive());
        }

        [TestMethod]
        public void Monster_TakeDamage_KillsMonster()
        {
            // Arrange
            var goblin = new Goblin();
            
            // Act
            goblin.TakeDamage(30);
            
            // Assert
            Assert.AreEqual(0, goblin.Health);
            Assert.IsFalse(goblin.IsAlive());
        }

        [TestMethod]
        public void Monster_Attack_DamagesTarget()
        {
            // Arrange
            var goblin = new Goblin();
            var player = new Player("TestPlayer");
            
            // Act
            goblin.Attack(player);
            
            // Assert
            Assert.AreEqual(95, player.Health);
        }

        [TestMethod]
        public void Monster_Attack_DeadTarget_NoAttack()
        {
            // Arrange
            var goblin = new Goblin();
            var player = new Player("TestPlayer");
            player.TakeDamage(100); // Kill player
            
            // Act
            goblin.Attack(player);
            
            // Assert
            Assert.AreEqual(0, player.Health); // No additional damage
        }

        [TestMethod]
        public void Dragon_Attack_HasDifferentBehavior()
        {
            // Arrange
            var dragon = new Dragon();
            var player = new Player("TestPlayer");
            
            // Act
            dragon.Attack(player);
            
            // Assert
            Assert.AreEqual(75, player.Health); // Dragon does 25 damage
        }

        #endregion

        #region Inventory Tests

        [TestMethod]
        public void Inventory_Constructor_InitializesEmpty()
        {
            // Arrange & Act
            var inventory = new Inventory();
            
            // Assert
            Assert.AreEqual(0, inventory.GetItems().Count);
            Assert.AreEqual(0, inventory.GetWeapons().Count);
            Assert.AreEqual(0, inventory.GetPotionsSortedByHealing().Count);
        }

        [TestMethod]
        public void Inventory_AddItem_AddsItemCorrectly()
        {
            // Arrange
            var inventory = new Inventory();
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            
            // Act
            inventory.AddItem(weapon);
            
            // Assert
            Assert.AreEqual(1, inventory.GetItems().Count);
            Assert.AreEqual(weapon, inventory.GetItems()[0]);
        }

        [TestMethod]
        public void Inventory_RemoveItem_RemovesItemCorrectly()
        {
            // Arrange
            var inventory = new Inventory();
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            inventory.AddItem(weapon);
            
            // Act
            inventory.RemoveItem(weapon);
            
            // Assert
            Assert.AreEqual(0, inventory.GetItems().Count);
        }

        [TestMethod]
        public void Inventory_GetWeapons_ReturnsOnlyWeapons()
        {
            // Arrange
            var inventory = new Inventory();
            var weapon1 = new Weapon { Name = "Sword", Damage = 10 };
            var weapon2 = new Weapon { Name = "Axe", Damage = 15 };
            var potion = new Potion { Name = "Health Potion", Healing = 20 };
            
            inventory.AddItem(weapon1);
            inventory.AddItem(potion);
            inventory.AddItem(weapon2);
            
            // Act
            var weapons = inventory.GetWeapons();
            
            // Assert
            Assert.AreEqual(2, weapons.Count);
            Assert.IsTrue(weapons.Contains(weapon1));
            Assert.IsTrue(weapons.Contains(weapon2));
            Assert.IsFalse(weapons.Any(w => w.Name == "Health Potion"));
        }

        [TestMethod]
        public void Inventory_GetPotionsSortedByHealing_ReturnsSortedPotions()
        {
            // Arrange
            var inventory = new Inventory();
            var potion1 = new Potion { Name = "Small Potion", Healing = 10 };
            var potion2 = new Potion { Name = "Large Potion", Healing = 30 };
            var potion3 = new Potion { Name = "Medium Potion", Healing = 20 };
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            
            inventory.AddItem(potion1);
            inventory.AddItem(weapon);
            inventory.AddItem(potion2);
            inventory.AddItem(potion3);
            
            // Act
            var potions = inventory.GetPotionsSortedByHealing();
            
            // Assert
            Assert.AreEqual(3, potions.Count);
            Assert.AreEqual(30, potions[0].Healing); // Large Potion
            Assert.AreEqual(20, potions[1].Healing); // Medium Potion
            Assert.AreEqual(10, potions[2].Healing); // Small Potion
        }

        [TestMethod]
        public void Inventory_GetItems_ReturnsDefensiveCopy()
        {
            // Arrange
            var inventory = new Inventory();
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            inventory.AddItem(weapon);
            
            // Act
            var items = inventory.GetItems();
            items.Clear(); // Try to modify the returned list
            
            // Assert
            Assert.AreEqual(1, inventory.GetItems().Count); // Original should be unchanged
        }

        #endregion

        #region Item Tests

        [TestMethod]
        public void Weapon_Collect_AddsToInventory()
        {
            // Arrange
            var player = new Player("TestPlayer");
            var weapon = new Weapon { Name = "Sword", Description = "A sharp blade", Damage = 10 };
            
            // Act
            weapon.Collect(player);
            
            // Assert
            Assert.AreEqual(1, player.Inventory.GetItems().Count);
            Assert.AreEqual(weapon, player.Inventory.GetItems()[0]);
        }

        [TestMethod]
        public void Potion_Collect_AddsToInventory()
        {
            // Arrange
            var player = new Player("TestPlayer");
            var potion = new Potion { Name = "Health Potion", Description = "Restores health", Healing = 20 };
            
            // Act
            potion.Collect(player);
            
            // Assert
            Assert.AreEqual(1, player.Inventory.GetItems().Count);
            Assert.AreEqual(potion, player.Inventory.GetItems()[0]);
        }

        [TestMethod]
        public void Potion_Use_HealsPlayer()
        {
            // Arrange
            var player = new Player("TestPlayer");
            player.TakeDamage(30);
            var potion = new Potion { Name = "Health Potion", Description = "Restores health", Healing = 20 };
            player.Inventory.AddItem(potion);
            
            // Act
            potion.Use(player);
            
            // Assert
            Assert.AreEqual(90, player.Health);
            Assert.AreEqual(0, player.Inventory.GetItems().Count); // Potion should be consumed
        }

        [TestMethod]
        public void Potion_Use_AtFullHealth_StillConsumesPotion()
        {
            // Arrange
            var player = new Player("TestPlayer");
            var potion = new Potion { Name = "Health Potion", Description = "Restores health", Healing = 20 };
            player.Inventory.AddItem(potion);
            
            // Act
            potion.Use(player);
            
            // Assert
            Assert.AreEqual(100, player.Health);
            Assert.AreEqual(0, player.Inventory.GetItems().Count); // Potion should still be consumed
        }

        [TestMethod]
        public void Item_ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var weapon = new Weapon { Name = "Sword", Description = "A sharp blade", Damage = 10 };
            
            // Act
            var result = weapon.ToString();
            
            // Assert
            Assert.AreEqual("Sword: A sharp blade", result);
        }

        [TestMethod]
        public void Weapon_Properties_SetCorrectly()
        {
            // Arrange & Act
            var weapon = new Weapon { Name = "Axe", Description = "A rusty axe", Damage = 15 };
            
            // Assert
            Assert.AreEqual("Axe", weapon.Name);
            Assert.AreEqual("A rusty axe", weapon.Description);
            Assert.AreEqual(15, weapon.Damage);
        }

        [TestMethod]
        public void Potion_Properties_SetCorrectly()
        {
            // Arrange & Act
            var potion = new Potion { Name = "Mega Potion", Description = "Super healing", Healing = 50 };
            
            // Assert
            Assert.AreEqual("Mega Potion", potion.Name);
            Assert.AreEqual("Super healing", potion.Description);
            Assert.AreEqual(50, potion.Healing);
        }

        #endregion

        #region Room Tests

        [TestMethod]
        public void Room_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var room = new Room("Test Room", "A test room description");
            
            // Assert
            Assert.AreEqual("Test Room", room.Name);
            Assert.AreEqual("A test room description", room.Description);
            Assert.AreEqual(0, room.Items.Count);
            Assert.AreEqual(0, room.Monsters.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Room_Constructor_EmptyName_ThrowsException()
        {
            // Act
            new Room("", "Description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Room_Constructor_EmptyDescription_ThrowsException()
        {
            // Act
            new Room("Name", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Room_Constructor_NullName_ThrowsException()
        {
            // Act
            new Room(null, "Description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Room_Constructor_WhitespaceDescription_ThrowsException()
        {
            // Act
            new Room("Name", "   ");
        }

        [TestMethod]
        public void Room_AddItem_AddsItemCorrectly()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            
            // Act
            room.AddItem(weapon);
            
            // Assert
            Assert.AreEqual(1, room.Items.Count);
            Assert.AreEqual(weapon, room.Items[0]);
        }

        [TestMethod]
        public void Room_AddItem_NullItem_DoesNotAdd()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            
            // Act
            room.AddItem(null);
            
            // Assert
            Assert.AreEqual(0, room.Items.Count);
        }

        [TestMethod]
        public void Room_RemoveItem_RemovesItemCorrectly()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            room.AddItem(weapon);
            
            // Act
            room.RemoveItem(weapon);
            
            // Assert
            Assert.AreEqual(0, room.Items.Count);
        }

        [TestMethod]
        public void Room_RemoveItem_NullItem_DoesNothing()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            room.AddItem(weapon);
            
            // Act
            room.RemoveItem(null);
            
            // Assert
            Assert.AreEqual(1, room.Items.Count);
        }

        [TestMethod]
        public void Room_AddMonster_AddsMonsterCorrectly()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var goblin = new Goblin();
            
            // Act
            room.AddMonster(goblin);
            
            // Assert
            Assert.AreEqual(1, room.Monsters.Count);
            Assert.AreEqual(goblin, room.Monsters[0]);
        }

        [TestMethod]
        public void Room_AddMonster_NullMonster_DoesNotAdd()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            
            // Act
            room.AddMonster(null);
            
            // Assert
            Assert.AreEqual(0, room.Monsters.Count);
        }

        [TestMethod]
        public void Room_RemoveMonster_RemovesMonsterCorrectly()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var goblin = new Goblin();
            room.AddMonster(goblin);
            
            // Act
            room.RemoveMonster(goblin);
            
            // Assert
            Assert.AreEqual(0, room.Monsters.Count);
        }

        [TestMethod]
        public void Room_AddMultipleItems_AddsAllItems()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            var potion = new Potion { Name = "Health Potion", Healing = 20 };
            
            // Act
            room.AddItem(weapon);
            room.AddItem(potion);
            
            // Assert
            Assert.AreEqual(2, room.Items.Count);
            Assert.IsTrue(room.Items.Contains(weapon));
            Assert.IsTrue(room.Items.Contains(potion));
        }

        #endregion

        #region GameMap Tests

        [TestMethod]
        public void GameMap_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var gameMap = new GameMap();
            
            // Assert
            Assert.IsNotNull(gameMap.Rooms);
            Assert.AreEqual(0, gameMap.Rooms.Count);
        }

        [TestMethod]
        public void GameMap_AddRoom_AddsRoomCorrectly()
        {
            // Arrange
            var gameMap = new GameMap();
            var room = new Room("Test Room", "Description");
            
            // Act
            gameMap.AddRoom(room);
            
            // Assert
            Assert.AreEqual(1, gameMap.Rooms.Count);
            Assert.IsTrue(gameMap.Rooms.ContainsKey("Test Room"));
            Assert.AreEqual(room, gameMap.Rooms["Test Room"]);
        }

        [TestMethod]
        public void GameMap_AddRoom_NullRoom_DoesNotAdd()
        {
            // Arrange
            var gameMap = new GameMap();
            
            // Act
            gameMap.AddRoom(null);
            
            // Assert
            Assert.AreEqual(0, gameMap.Rooms.Count);
        }

        [TestMethod]
        public void GameMap_AddRoom_DuplicateRoom_DoesNotAdd()
        {
            // Arrange
            var gameMap = new GameMap();
            var room1 = new Room("Test Room", "Description 1");
            var room2 = new Room("Test Room", "Description 2");
            
            // Act
            gameMap.AddRoom(room1);
            gameMap.AddRoom(room2);
            
            // Assert
            Assert.AreEqual(1, gameMap.Rooms.Count);
            Assert.AreEqual(room1, gameMap.Rooms["Test Room"]); // First room should remain
        }

        [TestMethod]
        public void GameMap_GetRoom_ReturnsCorrectRoom()
        {
            // Arrange
            var gameMap = new GameMap();
            var room = new Room("Test Room", "Description");
            gameMap.AddRoom(room);
            
            // Act
            var retrievedRoom = gameMap.GetRoom("Test Room");
            
            // Assert
            Assert.AreEqual(room, retrievedRoom);
        }

        [TestMethod]
        public void GameMap_GetRoom_NonExistentRoom_ReturnsNull()
        {
            // Arrange
            var gameMap = new GameMap();
            
            // Act
            var retrievedRoom = gameMap.GetRoom("Non-existent Room");
            
            // Assert
            Assert.IsNull(retrievedRoom);
        }

        [TestMethod]
        public void GameMap_GetRoom_CaseInsensitive_ReturnsCorrectRoom()
        {
            // Arrange
            var gameMap = new GameMap();
            var room = new Room("Test Room", "Description");
            gameMap.AddRoom(room);
            
            // Act
            var retrievedRoom = gameMap.GetRoom("test room");
            
            // Assert
            Assert.AreEqual(room, retrievedRoom);
        }

        [TestMethod]
        public void GameMap_AddMultipleRooms_AddsAllRooms()
        {
            // Arrange
            var gameMap = new GameMap();
            var room1 = new Room("Room 1", "Description 1");
            var room2 = new Room("Room 2", "Description 2");
            var room3 = new Room("Room 3", "Description 3");
            
            // Act
            gameMap.AddRoom(room1);
            gameMap.AddRoom(room2);
            gameMap.AddRoom(room3);
            
            // Assert
            Assert.AreEqual(3, gameMap.Rooms.Count);
            Assert.IsTrue(gameMap.Rooms.ContainsKey("Room 1"));
            Assert.IsTrue(gameMap.Rooms.ContainsKey("Room 2"));
            Assert.IsTrue(gameMap.Rooms.ContainsKey("Room 3"));
        }

        #endregion

        #region Creature Tests

        [TestMethod]
        public void Creature_IsAlive_PositiveHealth_ReturnsTrue()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act & Assert
            Assert.IsTrue(player.IsAlive());
        }

        [TestMethod]
        public void Creature_IsAlive_ZeroHealth_ReturnsFalse()
        {
            // Arrange
            var player = new Player("TestPlayer");
            player.TakeDamage(100);
            
            // Act & Assert
            Assert.IsFalse(player.IsAlive());
        }

        [TestMethod]
        public void Creature_IsAlive_NegativeHealth_ReturnsFalse()
        {
            // Arrange
            var player = new Player("TestPlayer");
            player.TakeDamage(150);
            
            // Act & Assert
            Assert.IsFalse(player.IsAlive());
        }

        [TestMethod]
        public void Creature_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var goblin = new Goblin("Test Goblin", 25, 8);
            
            // Assert
            Assert.AreEqual("Test Goblin", goblin.Name);
            Assert.AreEqual(25, goblin.Health);
        }

        [TestMethod]
        public void Creature_Name_CanBeModified()
        {
            // Arrange
            var goblin = new Goblin();
            
            // Act
            goblin.Name = "Modified Goblin";
            
            // Assert
            Assert.AreEqual("Modified Goblin", goblin.Name);
        }

        [TestMethod]
        public void Creature_Health_CanBeModified()
        {
            // Arrange
            var goblin = new Goblin();
            
            // Act
            goblin.Health = 50;
            
            // Assert
            Assert.AreEqual(50, goblin.Health);
        }

        #endregion

        #region Interface Tests

        [TestMethod]
        public void IDamageable_Player_ImplementsInterface()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act & Assert
            Assert.IsTrue(player is IDamageable);
            
            // Test interface method
            IDamageable damageable = player;
            damageable.TakeDamage(10);
            Assert.AreEqual(90, damageable.Health);
        }

        [TestMethod]
        public void IDamageable_Monster_ImplementsInterface()
        {
            // Arrange
            var goblin = new Goblin();
            
            // Act & Assert
            Assert.IsTrue(goblin is IDamageable);
            
            // Test interface method
            IDamageable damageable = goblin;
            damageable.TakeDamage(10);
            Assert.AreEqual(20, damageable.Health);
        }

        [TestMethod]
        public void ICollectible_Weapon_ImplementsInterface()
        {
            // Arrange
            var weapon = new Weapon { Name = "Sword", Damage = 10 };
            
            // Act & Assert
            Assert.IsTrue(weapon is ICollectible);
            
            // Test interface method
            ICollectible collectible = weapon;
            Assert.AreEqual("Sword", collectible.Name);
        }

        [TestMethod]
        public void ICollectible_Potion_ImplementsInterface()
        {
            // Arrange
            var potion = new Potion { Name = "Health Potion", Healing = 20 };
            
            // Act & Assert
            Assert.IsTrue(potion is ICollectible);
            
            // Test interface method
            ICollectible collectible = potion;
            Assert.AreEqual("Health Potion", collectible.Name);
        }

        #endregion

        #region LINQ Tests

        [TestMethod]
        public void LINQ_WeaponFiltering_WorksCorrectly()
        {
            // Arrange
            var inventory = new Inventory();
            var sword = new Weapon { Name = "Sword", Damage = 10 };
            var axe = new Weapon { Name = "Axe", Damage = 15 };
            var potion = new Potion { Name = "Health Potion", Healing = 20 };
            
            inventory.AddItem(sword);
            inventory.AddItem(potion);
            inventory.AddItem(axe);
            
            // Act
            var weapons = inventory.GetItems().OfType<Weapon>().ToList();
            
            // Assert
            Assert.AreEqual(2, weapons.Count);
            Assert.IsTrue(weapons.Any(w => w.Name == "Sword"));
            Assert.IsTrue(weapons.Any(w => w.Name == "Axe"));
        }

        [TestMethod]
        public void LINQ_PotionSorting_WorksCorrectly()
        {
            // Arrange
            var inventory = new Inventory();
            var smallPotion = new Potion { Name = "Small Potion", Healing = 10 };
            var largePotion = new Potion { Name = "Large Potion", Healing = 30 };
            var mediumPotion = new Potion { Name = "Medium Potion", Healing = 20 };
            
            inventory.AddItem(smallPotion);
            inventory.AddItem(largePotion);
            inventory.AddItem(mediumPotion);
            
            // Act
            var sortedPotions = inventory.GetItems()
                .OfType<Potion>()
                .OrderByDescending(p => p.Healing)
                .ToList();
            
            // Assert
            Assert.AreEqual(3, sortedPotions.Count);
            Assert.AreEqual(30, sortedPotions[0].Healing);
            Assert.AreEqual(20, sortedPotions[1].Healing);
            Assert.AreEqual(10, sortedPotions[2].Healing);
        }

        [TestMethod]
        public void LINQ_MonsterFiltering_WorksCorrectly()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var goblin1 = new Goblin("Goblin 1", 30, 5);
            var goblin2 = new Goblin("Goblin 2", 25, 5);
            var dragon = new Dragon();
            
            room.AddMonster(goblin1);
            room.AddMonster(goblin2);
            room.AddMonster(dragon);
            
            // Kill one goblin
            goblin1.TakeDamage(30);
            
            // Act
            var aliveMonsters = room.Monsters.Where(m => m.IsAlive()).ToList();
            
            // Assert
            Assert.AreEqual(2, aliveMonsters.Count);
            Assert.IsTrue(aliveMonsters.Any(m => m.Name == "Goblin 2"));
            Assert.IsTrue(aliveMonsters.Any(m => m.Name == "Ancient Dragon"));
        }

        [TestMethod]
        public void LINQ_StrongestMonster_WorksCorrectly()
        {
            // Arrange
            var room = new Room("Test Room", "Description");
            var goblin = new Goblin("Weak Goblin", 20, 3);
            var strongGoblin = new Goblin("Strong Goblin", 40, 8);
            var dragon = new Dragon();
            
            room.AddMonster(goblin);
            room.AddMonster(strongGoblin);
            room.AddMonster(dragon);
            
            // Act
            var strongest = room.Monsters
                .Where(m => m.IsAlive())
                .OrderByDescending(m => m.AttackDamage)
                .FirstOrDefault();
            
            // Assert
            Assert.IsNotNull(strongest);
            Assert.AreEqual("Ancient Dragon", strongest.Name);
            Assert.AreEqual(25, strongest.AttackDamage);
        }

        #endregion

        #region Edge Cases and Error Handling

        [TestMethod]
        public void Room_DisplayItems_EmptyRoom_DoesNotCrash()
        {
            // Arrange
            var room = new Room("Empty Room", "Description");
            
            // Act & Assert - Should not throw exception
            room.DisplayItems();
        }

        [TestMethod]
        public void Room_DisplayMonsters_EmptyRoom_DoesNotCrash()
        {
            // Arrange
            var room = new Room("Empty Room", "Description");
            
            // Act & Assert - Should not throw exception
            room.DisplayMonsters();
        }

        [TestMethod]
        public void Inventory_DisplayInventory_EmptyInventory_DoesNotCrash()
        {
            // Arrange
            var inventory = new Inventory();
            
            // Act & Assert - Should not throw exception
            inventory.DisplayInventory();
        }

        [TestMethod]
        public void Player_DisplayStatus_DoesNotCrash()
        {
            // Arrange
            var player = new Player("TestPlayer");
            
            // Act & Assert - Should not throw exception
            player.DisplayStatus();
        }

        [TestMethod]
        public void Monster_Attack_SelfAttack_WorksCorrectly()
        {
            // Arrange
            var goblin1 = new Goblin("Goblin 1", 30, 5);
            var goblin2 = new Goblin("Goblin 2", 25, 5);
            
            // Act
            goblin1.Attack(goblin2);
            
            // Assert
            Assert.AreEqual(20, goblin2.Health);
        }

        #endregion
    }
}