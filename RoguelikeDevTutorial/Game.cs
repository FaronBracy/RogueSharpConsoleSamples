using System;
using System.Collections.Generic;
using RogueSharp;
using RogueSharp.ConsoleEngine;
using RogueSharp.MapCreation;

namespace RoguelikeDevTutorial
{
   public static class Game
   {
      public static RSWindow MainWindow { get; private set; }

      public static Engine Engine { get; private set; }

      public static void Main( string[] args )
      {
         int screenWidth = 80;
         int screenHeight = 50;

         int mapWidth = 80;
         int mapHeight = 45;

         int playerX = screenWidth / 2;
         int playerY = screenHeight / 2;

         Entity player = new Entity( playerX, playerY, '@', RSColor.White );
         Entity npc = new Entity( playerX - 5, playerY, '@', new RSColor( 255, 255, 0 ) );

         InputHandler inputHandler = new InputHandler();

         // BitmapFont in RogueSharp is like the tileset in TCOD
         BitmapFont tileset = new BitmapFont( 10, 10, 32, 8, "dejavu10x10_gs_tc.png", BitmapFontLayout.Tcod );

         // The font for the tutorial doesn't really turn my crank
         // For a much better font use a CP437 layout one from RexPaint such as one here
         // https://www.gridsagegames.com/rexpaint/resources.html#Fonts
         // BitmapFont tileset = new BitmapFont( 10, 10, 16, 16, "font_10x10.png", BitmapFontLayout.Cp437 );

         // RSWindow is roughly equivalent to TCOD Terminal or Context
         MainWindow = new RSWindow( tileset, screenWidth, screenHeight, "Yet Another Roguelike Tutorial" );
         MainWindow.Quitting += MainWindowQuitting;
         MainWindow.KeyDown += MainWindowKeyDown;
         MainWindow.Render += MainWindowRender;
         MainWindow.Update += MainWindowUpdate;

         // RogueSharp has built in map creation. RandomRooms matches what the tutorial is doing.
         //RandomRoomsMapCreationStrategy<GameMap, Tile> strategy = new(mapWidth, mapHeight, 30, 10, 6);

         DungeonCreationStrategy dungeonCreationStrategy = new( mapWidth, mapHeight );
         GameMap gameMap = Map.Create( dungeonCreationStrategy );

         //foreach ( Tile tile in gameMap.GetAllCells() )
         //{
         //   gameMap.SetTileData( tile, tile.IsWalkable ? Tile.Floor : Tile.Wall );
         //}

         Engine = new Engine( new List<Entity> { player, npc }, inputHandler, player, gameMap );

         // Kick off the main game loop
         MainWindow.Start();
      }

      private static void MainWindowQuitting( object sender, EventArgs e )
      {
         // Clean up other resources here if necessary
         Console.WriteLine( "Quitting" );
      }

      private static void MainWindowKeyDown( object sender, KeyEventArgs e )
      {
         Console.WriteLine( $"KeyDown - {e.Key.KeyCode} - {e.Key.KeyScanCode} - {e.Key.KeyModifier}" );

         Engine.HandleInput( e.Key );
      }

      private static void MainWindowRender( object sender, FrameEventArgs e )
      {
         Engine.Render( MainWindow );
      }

      private static void MainWindowUpdate( object sender, FrameEventArgs e )
      {
      }
   }
}
