from player import Player
from room import Room

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
            self.player.display_status()

            item = self.rooms[current_room].get_item()
            if item:
                print(f"You see a {item} in the room.")
                pick = input("Do you want to pick it up? (yes/no): ").lower()
                if pick == "yes":
                    self.player.pick_up_item(item)

            import random
            if random.choice([True, False]):  # 50% chance of danger
                damage = random.randint(5, 20)
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