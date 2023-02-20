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
         tileToSet.IsExplored = tileTemplate.IsExplored;
         tileToSet.IsVisible = tileTemplate.IsVisible;
         tileToSet.Dark = tileTemplate.Dark;
         tileToSet.Light = tileTemplate.Light;
         tileToSet.Shroud = tileTemplate.Shroud;
      }

      public bool InBounds( int x, int y )
      {
         return 0 <= x && x < Width && 0 <= y && 0 < Height;
      }

      public void Render( RSWindow mainWindow )
      {
         foreach( Tile tile in GetAllCells() )
         {
            if ( tile.IsVisible )
            {
               mainWindow.RootConsole.Set( tile.X, tile.Y, tile.Light.Foreground, tile.Light.Background, tile.Light.Character );
            }
            else if ( tile.IsExplored )
            {
               mainWindow.RootConsole.Set( tile.X, tile.Y, tile.Dark.Foreground, tile.Dark.Background, tile.Dark.Character );
            }
            else
            {
               mainWindow.RootConsole.Set( tile.X, tile.Y, tile.Shroud.Foreground, tile.Shroud.Background, tile.Shroud.Character );
            }
         }
      }
   }
}