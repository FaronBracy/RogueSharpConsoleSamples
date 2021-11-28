using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Graphic
   {
      public char Character { get; set; }
      public RSColor Foreground { get; set; }
      public RSColor Background { get; set; }
   }

   public class Tile : Cell
   {
      public static Tile Floor => new Tile( true, true, new Graphic { Character = ' ', Foreground = new RSColor( 255, 255, 255 ), Background = new RSColor( 50, 50, 150 ) } );
      public static Tile Wall => new Tile( true, false, new Graphic { Character = ' ', Foreground = new RSColor( 255, 255, 255 ), Background = new RSColor( 0, 0, 100 ) } );

      public Graphic Dark { get; set; }

      public Tile( int x, int y, bool isTransparent, bool isWalkable, Graphic dark )
         : base( x, y, isTransparent, isWalkable )
      {
         Dark = dark;
      }

      public Tile( bool isTransparent, bool isWalkable, Graphic dark )
         : base( 0, 0, isTransparent, isWalkable )
      {
         Dark = dark;
      }

      public Tile()
      {
         // required empty default constructor
      }
   }

   public class Floor : Tile
   {
      public Floor()
         : base( 0, 0, true, true, new Graphic { Character = ' ', Foreground = new RSColor( 255, 255, 255 ), Background = new RSColor( 50, 50, 150 ) } )
      {
      }
   }

   public class Wall : Tile
   {
      public Wall( int x, int y )
         : base( x, y, true, true, new Graphic { Character = ' ', Foreground = new RSColor( 255, 255, 255 ), Background = new RSColor( 0, 0, 100 ) } )
      {
      }
   }
}
