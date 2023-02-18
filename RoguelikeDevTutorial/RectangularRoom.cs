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

      public Point Center
      {
         get
         {
            int centerX = ( X1 + X2 ) / 2;
            int centerY = ( Y1 + Y2 ) / 2;
            return new Point( centerX, centerY );
         }
      }

      public (Point upperLeft, Point lowerRight) Inner
      {
         get
         {
            Point upperLeft = new Point( X1 + 1, Y1 + 1 );
            Point lowerRight = new Point( X2 - 1, Y2 - 1 );
            return (upperLeft, lowerRight);
         }
      }

      public RectangularRoom( int x1, int y1, int width, int height )
      {
         X1 = x1;
         Y1 = y1;
         Width = width;
         Height = height;
      }
   }
}
