using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class GameMap : Map<Tile>
   {
      public GameMap( int width, int height )
         : base( width, height )
      {
         foreach( Tile tile in GetAllCells() )
         {
            SetTileData( tile, Tile.Wall );
         }
      }

      public GameMap()
      {
         // required empty default constructor
      }

      public void SetTileData( Tile tileToSet, Tile tileTemplate )
      {
         tileToSet.IsTransparent = tileTemplate.IsTransparent;
         tileToSet.IsWalkable = tileTemplate.IsWalkable;
         tileToSet.Dark = tileTemplate.Dark;
      }

      public bool InBounds( int x, int y )
      {
         return 0 <= x && x < Width && 0 <= y && 0 < Height;
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