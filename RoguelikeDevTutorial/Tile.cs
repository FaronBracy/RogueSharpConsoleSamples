using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization.Formatters;
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
      public static Tile Floor()
      {
         return new Tile( 0, 0, true, true, new Graphic { Character = ' ', Foreground = new RSColor( 255, 255, 255 ), Background = new RSColor( 50, 50, 150 ) } );
      }
      public static Tile Wall()
      {
         return new Tile( 0, 0, true, true, new Graphic { Character = ' ', Foreground = new RSColor( 255, 255, 255 ), Background = new RSColor( 0, 0, 100 ) } );
      }

      public Graphic Dark { get; set; }

      public Tile( int x, int y, bool isTransparent, bool isWalkable, Graphic dark )
         : base( x, y, isTransparent, isWalkable )
      {
         Dark = dark;
      }

      public Tile()
      {
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

   public class GameMap : Map<Tile>
   {
      public GameMap( int width, int height )
         : base( width, height )
      {
         foreach( Tile tile in GetAllCells() )
         {
            SetTileData( tile, Tile.Floor() );
         }
      }

      private void SetTileData( Tile source, Tile destination )
      {
         source.IsTransparent = destination.IsTransparent;
         source.IsWalkable = destination.IsWalkable;
         source.Dark = destination.Dark;
      }

      public void Render( RSWindow mainWindow )
      {
         foreach( Tile tile in GetAllCells() )
         {
            mainWindow.RootConsole.Set( tile.X, tile.Y, tile.Dark.Foreground, tile.Dark.Background, tile.Dark.Character );  
         }
      }
   }
}
