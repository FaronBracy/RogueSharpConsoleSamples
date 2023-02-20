using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Tile : Cell
   {
      private static readonly RSColor White = new(255, 255, 255);
      private static readonly RSColor Black = new(0, 0, 0);
      private static readonly RSColor FloorDarkColor = new( 50, 50, 150 );
      private static readonly RSColor FloorLightColor = new( 200, 180, 50 );
      private static readonly RSColor WallDarkColor = new( 0, 0, 100 );
      private static readonly RSColor WallLightColor = new( 130, 110, 50 );

      public static Tile Floor => new Tile( true, true
         , new RSCell( ' ', White, FloorDarkColor )
         , new RSCell( ' ', White, FloorLightColor ) );

      public static Tile Wall => new Tile( false, false
         , new RSCell (' ', White,  WallDarkColor )
         , new RSCell( ' ', White, WallLightColor ) );
      
      public bool IsVisible { get; set; }
      public bool IsExplored { get; set; }
      public RSCell Dark { get; set; }
      public RSCell Light { get; set; }
      public RSCell Shroud { get; set; }

      public Tile( int x, int y, bool isTransparent, bool isWalkable, RSCell dark, RSCell light )
         : base( x, y, isTransparent, isWalkable )
      {
         Dark = dark;
         Light = light;
         Shroud = new RSCell( ' ', Black, Black );
      }

      public Tile( bool isTransparent, bool isWalkable, RSCell dark, RSCell light )
         : base( 0, 0, isTransparent, isWalkable )
      {
         Dark = dark;
         Light = light;
         Shroud = new RSCell( ' ', Black, Black );
      }

      public Tile()
      {
         // required empty default constructor
      }
   }
}
