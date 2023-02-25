using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Entity
   {
      public static Entity Player => new( 0, 0, '@', new RSColor( 255, 255, 255 ), "Player", true );
      public static Entity Orc => new( 0, 0, 'o', new RSColor( 63, 127, 63 ), "Orc", true );
      public static Entity Troll => new( 0, 0, 'T', new RSColor( 0, 127, 0 ), "Troll", true );

      public int X { get; set; }
      public int Y { get; set; }
      public char Character { get; set; }
      public RSColor Color { get; set; }
      public string Name { get; set; }
      public bool BlocksMovement { get; set; }

      public Entity( int x, int y, char character, RSColor color, string name, bool blocksMovement )
      {
         X = x;
         Y = y;
         Character = character;
         Color = color;
         Name = name;
         BlocksMovement = blocksMovement;
      }

      public void Move( int dx, int dy )
      {
         X += dx;
         Y += dy;
      }

      public Entity Spawn( GameMap gameMap, int x, int y )
      {
         Entity clone = new Entity( x, y, Character, Color, Name, BlocksMovement );
         gameMap.Entities.Add( clone );
         return clone;
      }
   }
}
