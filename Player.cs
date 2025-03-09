class Player:
    def __init__(self, name):
        self.__name = name
        self.__inventory = []
        self.__health = 100  # Encapsulated Health Attribute

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
            self.__health = 0
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