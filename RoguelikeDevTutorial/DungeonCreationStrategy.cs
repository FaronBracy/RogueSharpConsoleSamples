using RogueSharp;
using RogueSharp.DiceNotation;
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
         TunnelBetween( room1.Center, room2.Center );

         return _dungeon;
      }

      private void DigRoom( RectangularRoom room )
      {
         foreach ( Tile tile in _dungeon.GetCellsInRectangle( room.Inner.Top, room.Inner.Left, room.Width, room.Height ) )
         {
            _dungeon.SetTileData( tile, Tile.Floor );
         }
      }

      private void TunnelBetween( Point start, Point end )
      {
         Point corner;

         //// Alternative way to set up dice in RogueSharp
         //DiceExpression d100 = new DiceExpression().Die( 100 );
         //int result = d100.Roll().Value;

         corner = Dice.Roll( "d100" ) < 50 ? new Point( end.X, start.Y ) : new Point( start.X, end.Y );

         foreach ( Tile tile in _dungeon.GetCellsAlongLine( start.X, start.Y, corner.X, corner.Y ) )
         {
            _dungeon.SetTileData( tile, Tile.Floor );
         }
         foreach ( Tile tile in _dungeon.GetCellsAlongLine( end.X, end.Y, corner.X, corner.Y ) )
         {
            _dungeon.SetTileData( tile, Tile.Floor );
         }
      }
   }
}
