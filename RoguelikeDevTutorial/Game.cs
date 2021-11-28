using System;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public static class Game
   {
      public static RSWindow MainWindow { get; private set; }
      public static Entity Player { get; private set; }
      public static InputHandler InputHandler { get; private set; }

      public static void Main( string[] args )
      {
         int screenWidth = 80;
         int screenHeight = 50;

         int playerX = screenWidth / 2;
         int playerY = screenHeight / 2;

         Player = new Entity( playerX, playerY, '@', RSColor.White );

         InputHandler = new InputHandler();

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

         //int i = 0;
         //int[] charMapTcod = { 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 91, 92, 93, 94, 95, 96, 123, 124, 125, 126, 9617, 9618, 9619, 9474, 9472, 9532, 9508, 9524, 9500, 9516, 9492, 9484, 9488, 9496, 9624, 9629, 9600, 9622, 9626, 9616, 9623, 8593, 8595, 8592, 8594, 9650, 9660, 9668, 9658, 8597, 8596, 9744, 9745, 9675, 9673, 9553, 9552, 9580, 9571, 9577, 9568, 9574, 9562, 9556, 9559, 9565, 0, 0, 0, 0, 0, 0, 0, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 0, 0, 0, 0, 0, 0, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 0, 0, 0, 0, 0, 0 };
         //for ( int y = 0; y < 8; y++ )
         //{
         //   for ( int x = 0; x < 32; x++ )
         //   {
         //      MainWindow.RootConsole.Set( x, y, RSColor.Yellow, RSColor.Magenta, charMapTcod[i] );
         //      if ( i < charMapTcod.Length - 1 )
         //      {
         //         i++;
         //      }
         //   }
         //}
         
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

         IAction action = InputHandler.HandleKey( e.Key );
         action.Execute();
      }

      private static void MainWindowRender( object sender, FrameEventArgs e )
      {
         MainWindow.RootConsole.Clear();
         MainWindow.RootConsole.Print( Player.X, Player.Y, "@", RSColor.White );
         MainWindow.Draw();
      }

      private static void MainWindowUpdate( object sender, FrameEventArgs e )
      {
      }
   }
}
