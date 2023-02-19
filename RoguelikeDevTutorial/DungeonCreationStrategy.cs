using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using RogueSharp.DiceNotation;
using RogueSharp.MapCreation;
using RogueSharp.Random;

namespace RoguelikeDevTutorial
{
   public class DungeonCreationStrategy : IMapCreationStrategy<GameMap,Tile>
   {
      public int Width { get; }
      public int Height { get; }
      public int MaxRooms { get; }
      public int RoomMinSize { get; }
      public int RoomMaxSize { get; }
      public Entity Player { get; }
      public List<RectangularRoom> Rooms { get; }

      private GameMap _dungeon;

      public DungeonCreationStrategy( int width, int height, int maxRooms, int roomMinSize, int roomMaxSize, Entity player )
      {
         Width = width;
         Height = height;
         MaxRooms = maxRooms;
         RoomMinSize = roomMinSize;
         RoomMaxSize = roomMaxSize;
         Player = player;
         Rooms = new List<RectangularRoom>();
      }

      public GameMap CreateMap()
      {
         IRandom random = new DotNetRandom();

         _dungeon = new GameMap( Width, Height );
         for ( int i = 0; i < MaxRooms; i++ )
         {
            int roomWidth = random.Next( RoomMinSize, RoomMaxSize );
            int roomHeight = random.Next( RoomMinSize, RoomMaxSize );

            int x = random.Next( 0, Width - roomWidth - 1 );
            int y = random.Next( 0, Height - roomHeight - 1 );

            RectangularRoom newRoom = new RectangularRoom( x, y, roomWidth, roomHeight );

            if ( Rooms.Any( r => r.Intersects( newRoom ) ) )
            {
               continue;
            }

            Rectangle inner = newRoom.Inner;
            foreach ( Tile tile in _dungeon.GetCellsInRectangle( inner.Top, inner.Left, inner.Width, inner.Height ) )
            {
               _dungeon.SetTileData( tile, Tile.Floor );
            }

            if ( Rooms.Count == 0 )
            {
               Player.X = newRoom.Center.X;
               Player.Y = newRoom.Center.Y;
            }
            else
            {
               TunnelBetween( Rooms.Last().Center, newRoom.Center );
            }

            Rooms.Add( newRoom );
         }

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
