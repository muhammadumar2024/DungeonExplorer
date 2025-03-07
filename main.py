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
        print(f"\n{self.player.name}, you are entering the dungeon!")
        current_room = "Entrance"

        while True:
            print(f"\n{self.rooms[current_room].get_description()}")

            # Check if there is an item in the room
            item = self.rooms[current_room].get_item()
            if item:
                print(f"You see a {item} in the room.")
                pick = input("Do you want to pick it up? (yes/no): ").lower()
                if pick == "yes":
                    self.player.pick_up_item(item)

            action = input("Type 'move' to go to another room or 'quit' to exit: ").lower()

            if action == "move":
                next_room = input("Enter the room name (Hall/Treasure Room): ")
                if next_room in self.rooms:
                    current_room = next_room
                else:
                    print("⚠️ You cannot enter a room that doesn’t exist! Try again.")
            elif action == "quit":
                print("Exiting the game. Goodbye!")
                break
            else:
                print("⚠️ Invalid command! Use 'move' or 'quit'.")


class Player:
    def __init__(self, name):
        self.name = name
        self.__inventory = []  # Private attribute

    def pick_up_item(self, item):
        """Adds an item to the player's inventory."""
        self.__inventory.append(item)
        print(f"{self.name} picked up {item}!")

    def show_inventory(self):
        """Displays the player's inventory."""
        if self.__inventory:
            print(f"{self.name}'s Inventory: {', '.join(self.__inventory)}")
        else:
            print(f"{self.name} has an empty inventory.")


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