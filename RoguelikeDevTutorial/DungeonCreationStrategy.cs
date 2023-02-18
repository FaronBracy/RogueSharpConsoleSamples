using System;
using RogueSharp.MapCreation;

namespace RoguelikeDevTutorial
{
   public class DungeonCreationStrategy : IMapCreationStrategy<GameMap,Tile>
   {
      public int Width { get; }
      public int Height { get; }
      private GameMap _dungeon;

      public DungeonCreationStrategy( int width, int height )
      {
         Width = width;
         Height = height;
      }

      public GameMap CreateMap()
      {
         _dungeon = new GameMap( Width, Height );
         RectangularRoom room1 = new RectangularRoom( 20, 15, 10, 15 );
         RectangularRoom room2 = new RectangularRoom( 35, 15, 10, 15 );

         DigRoom( room1 );
         DigRoom( room2 );

         return _dungeon;
      }

      private void DigRoom( RectangularRoom room )
      {
         Console.WriteLine( "Digging Room" );
         foreach ( Tile tile in _dungeon.GetCellsInRectangle( room.Inner.Top, room.Inner.Left, room.Width, room.Height ) )
         {
            _dungeon.SetTileData( tile, Tile.Floor );
         }
      }
   }
}
