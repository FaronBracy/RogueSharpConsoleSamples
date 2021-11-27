using System;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public static class Game
   {
      public static RSWindow MainWindow { get; private set; }
      public static Actor Player { get; private set; }
      public static InputHandler InputHandler { get; private set; }

      public static void Main( string[] args )
      {
         int screenWidth = 80;
         int screenHeight = 50;

         int playerX = screenWidth / 2;
         int playerY = screenHeight / 2;

         Player = new Actor( playerX, playerY );

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
