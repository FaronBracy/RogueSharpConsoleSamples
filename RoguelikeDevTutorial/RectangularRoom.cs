using RogueSharp;

namespace RoguelikeDevTutorial
{
   public class RectangularRoom
   {
      public int X1 { get; set; }
      public int Y1 { get; set; }
      public int X2 { get; set; }
      public int Y2 { get; set; }

      public Point Center
      {
         get
         {
            int centerX = ( X1 + X2 ) / 2;
            int centerY = ( Y1 + Y2 ) / 2;
            return new Point( centerX, centerY );
         }
      }
      public (Point, Point) Inner
      {
         get
         {
            Point xRange = new Point( X1 + 1, X2 );
            Point yRange = new Point( Y1 + 1, Y2 );
            return (xRange, yRange);
         }
      }
      public RectangularRoom( int x1, int y1, int x2, int y2 )
      {
         X1 = x1;
         Y1 = y1;
         X2 = x2;
         Y2 = y2;
      }
   }
}
