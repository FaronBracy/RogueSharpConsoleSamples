namespace RoguelikeDevTutorial
{
   public interface IEntity
   {
      int X { get; set; }
      int Y { get; set; }

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

      public Entity( int x, int y )
      {
         X = x;
         Y = y;
      }

      public void Move( int dx, int dy )
      {
         X += dx;
         Y += dy;
      }
   }
}
