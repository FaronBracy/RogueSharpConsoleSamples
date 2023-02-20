using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Tile : Cell
   {
      private static readonly RSColor White = new(255, 255, 255);
      private static readonly RSColor FloorColor = new( 50, 50, 150 );
      private static readonly RSColor WallColor = new( 0, 0, 100 );

      public static Tile Floor => new Tile( true, true, new RSCell( ' ', White, FloorColor ) );
      public static Tile Wall => new Tile( false, false, new RSCell (' ', White,  WallColor ) );

      public RSCell Dark { get; set; }

      public Tile( int x, int y, bool isTransparent, bool isWalkable, RSCell dark )
         : base( x, y, isTransparent, isWalkable )
      {
         Dark = dark;
      }

      public Tile( bool isTransparent, bool isWalkable, RSCell dark )
         : base( 0, 0, isTransparent, isWalkable )
      {
         Dark = dark;
      }

      public Tile()
      {
         // required empty default constructor
      }
   }
}
