using System.Collections.Generic;

namespace DungeonExplorer
{
    public class GameMap
    {
        public Dictionary<string, Room> Rooms { get; private set; } = new Dictionary<string, Room>(System.StringComparer.OrdinalIgnoreCase);

        public void AddRoom(Room room)
        {
            if (room != null && !Rooms.ContainsKey(room.Name))
            {
                Rooms[room.Name] = room;
            }
        }

        public Room GetRoom(string name)
        {
            Rooms.TryGetValue(name, out Room room);
            return room;
        }
    }
}