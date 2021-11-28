using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public interface IEntity
   {
      int X { get; set; }
      int Y { get; set; }
      char Character { get; set;}
      RSColor Color { get; set; }

      void Move( int dx, int dy );
   }

   public class Entity : IEntity
   {
      public int X
      {
         get;
         set;
      }

      public int Y
      {
         get;
         set;
      }

      public char Character
      {
         get;
         set;
      }

      public RSColor Color
      {
         get;
         set;
      }

      public Entity( int x, int y, char character, RSColor color )
      {
         X = x;
         Y = y;
         Character = character;
         Color = color;
      }

      public void Move( int dx, int dy )
      {
         X += dx;
         Y += dy;
      }
   }
}
