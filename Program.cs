using System;

namespace DungeonExplorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
            Console.WriteLine("\nGame Over.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
