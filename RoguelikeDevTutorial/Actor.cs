namespace RoguelikeDevTutorial
{
   public interface IActor
   {
      int X { get; set; }
      int Y { get; set; }

      void Move( int dx, int dy );
   }

   public class Actor : IActor
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

      public Actor( int x, int y )
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
