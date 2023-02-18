using RogueSharp;

namespace RoguelikeDevTutorial
{
   public class RectangularRoom
   {
      public int X1 { get; }
      public int Y1 { get; }
      public int Width { get; }
      public int Height { get; }

      public int X2 => X1 + Width;
      public int Y2 => Y1 + Height;

      public RectangularRoom( int xLeft, int yTop, int width, int height )
      {
         X1 = xLeft;
         Y1 = yTop;
         Width = width;
         Height = height;
      }

      public Point Center
      {
         get
         {
            int centerX = ( X1 + X2 ) / 2;
            int centerY = ( Y1 + Y2 ) / 2;
            return new Point( centerX, centerY );
         }
      }

      public Rectangle Inner => new Rectangle(X1 + 1, Y1 + 1, Width - 2, Height - 2 );
   }
}
