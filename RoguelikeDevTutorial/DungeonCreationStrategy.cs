using System;
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
      public int MaxMonstersPerRoom { get;  }
      public Entity Player { get; }
      public List<RectangularRoom> Rooms { get; }

      private GameMap _dungeon;

      public DungeonCreationStrategy( int width, int height, int maxRooms, int roomMinSize, int roomMaxSize, int maxMonstersPerRoom, Entity player )
      {
         Width = width;
         Height = height;
         MaxRooms = maxRooms;
         RoomMinSize = roomMinSize;
         RoomMaxSize = roomMaxSize;
         MaxMonstersPerRoom = maxMonstersPerRoom;
         Player = player;
         Rooms = new List<RectangularRoom>();
      }

      public GameMap CreateMap()
      {
         _dungeon = new GameMap( Width, Height );
         _dungeon.Entities.Add( Player );
         for ( int i = 0; i < MaxRooms; i++ )
         {
            int roomWidth = Game.Random.Next( RoomMinSize, RoomMaxSize );
            int roomHeight = Game.Random.Next( RoomMinSize, RoomMaxSize );

            int x = Game.Random.Next( 0, Width - roomWidth - 1 );
            int y = Game.Random.Next( 0, Height - roomHeight - 1 );

            RectangularRoom newRoom = new RectangularRoom( x, y, roomWidth, roomHeight );

            if ( Rooms.Any( r => r.Intersects( newRoom ) ) )
            {
               continue;
            }

            DigRoom( newRoom );

            if ( Rooms.Count == 0 )
            {
               Player.X = newRoom.Center.X;
               Player.Y = newRoom.Center.Y;
            }
            else
            {
               PlaceEntities( newRoom );
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

      private void PlaceEntities( RectangularRoom room )
      {
         int numberOfMonsters = Game.Random.Next( 0, MaxMonstersPerRoom );

         for ( int i = 0; i < numberOfMonsters; i++ )
         {
            int x = Game.Random.Next( room.Inner.Left, room.Inner.Right );
            int y = Game.Random.Next( room.Inner.Top, room.Inner.Bottom );

            if ( !_dungeon.Entities.Any( e => e.X == x && e.Y == y ) )
            {
               if ( Game.Random.Next( 1, 100 ) < 80 )
               {
                  Console.WriteLine($"Spawn orc at {x}, {y}");
                  // Place an Orc here
                  Entity.Orc.Spawn( _dungeon, x, y );
               }
               else
               {
                  Console.WriteLine( $"Spawn troll at {x}, {y}" );
                  // Place a Troll here
                  Entity.Troll.Spawn( _dungeon, x, y );
               }
            }
         }
      }
   }
}
