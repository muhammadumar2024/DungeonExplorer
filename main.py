class Game:
    def __init__(self):
        self.player = None
        self.rooms = {}

    def start(self):
        print("Welcome to Dungeon Explorer!")
        name = input("Enter your name: ")
        self.player = Player(name)  # Create a player object
        self.create_rooms()
        self.play()

    def create_rooms(self):
        """Creates rooms and assigns items."""
        self.rooms["Entrance"] = Room("You are at the dungeon entrance.", "Torch")
        self.rooms["Hall"] = Room("A long, dark hallway with flickering torches.", "Shield")
        self.rooms["Treasure Room"] = Room("A bright room filled with gold and jewels.", "Gold Coin")

    def play(self):
        """Controls the game flow."""
        print(f"\n{self.player.get_name()}, you are entering the dungeon!")
        current_room = "Entrance"

        while True:
            print(f"\n{self.rooms[current_room].get_description()}")
            self.player.display_status()  # ✅ Display player status after every move

            # Check if there is an item in the room
            item = self.rooms[current_room].get_item()
            if item:
                print(f"You see a {item} in the room.")
                pick = input("Do you want to pick it up? (yes/no): ").lower()
                if pick == "yes":
                    self.player.pick_up_item(item)

            # Simulate a random danger
            import random
            if random.choice([True, False]):  # 50% chance of danger
                damage = random.randint(5, 20)  # Random damage between 5-20
                print(f"⚠️ Oh no! You encountered a trap and lost {damage} health.")
                self.player.reduce_health(damage)

            action = input("Type 'move' to go to another room or 'quit' to exit: ").lower()

            if action == "move":
                while True:
                    next_room = input("Enter the room name (Hall/Treasure Room): ")
                    if next_room in self.rooms:
                        current_room = next_room
                        break
                    else:
                        print("⚠️ Invalid room! Please enter a valid room name.")
            elif action == "quit":
                print("Exiting the game. Goodbye!")
                break
            else:
                print("⚠️ Invalid command! Use 'move' or 'quit'.")


class Player:
    def __init__(self, name):
        self.__name = name
        self.__inventory = []  # Private attribute for inventory
        self.__health = 100  # ✅ Encapsulated Health Attribute

    def get_name(self):
        """Returns the player's name."""
        return self.__name

    def get_health(self):
        """Getter for health."""
        return self.__health

    def reduce_health(self, amount):
        """Reduces player health when encountering dangers."""
        self.__health -= amount
        if self.__health < 0:
            self.__health = 0  # Prevents negative health
        print(f"⚠️ {self.__name}'s health reduced by {amount}. Current health: {self.__health}")

    def pick_up_item(self, item):
        """Adds an item to the player's inventory."""
        self.__inventory.append(item)
        print(f"{self.__name} picked up {item}!")

    def show_inventory(self):
        """Displays the player's inventory."""
        if self.__inventory:
            print(f"{self.__name}'s Inventory: {', '.join(self.__inventory)}")
        else:
            print(f"{self.__name} has an empty inventory.")

    def display_status(self):
        """Displays player health and inventory in one method."""
        print(f"\n💖 Health: {self.__health}")
        self.show_inventory()


class Room:
    def __init__(self, description, item=None):
        self.__description = description
        self.__item = item  # Item available in the room

    def get_description(self):
        """Returns the room's description."""
        return self.__description

    def get_item(self):
        """Returns the item in the room (if any)."""
        return self.__item


# Run the game
if __name__ == "__main__":
    game = Game()
    game.start()