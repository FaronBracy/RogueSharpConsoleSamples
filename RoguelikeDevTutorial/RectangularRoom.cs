using RogueSharp;

namespace RoguelikeDevTutorial
{
   public class RectangularRoom
   {
      public int Left => Outer.Left;
      public int Top => Outer.Top;
      public int Width => Outer.Width;
      public int Height => Outer.Height;

      public Rectangle Outer { get; }

      public RectangularRoom( int xLeft, int yTop, int width, int height )
      {
         Outer = new Rectangle( xLeft, yTop, width, height );
      }

      public Point Center => Outer.Center;

      public Rectangle Inner => new(Left + 1, Top + 1, Width - 2, Height - 2 );
   }
}
