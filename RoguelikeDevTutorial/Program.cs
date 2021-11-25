using System;
using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Program
   {
      public static RSWindow MainWindow { get; private set; }

      public static void Main( string[] args )
      {
         int screenWidth = 80;
         int screenHeight = 50;

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

         MainWindow.RootConsole.Print( 1, 1, "@", RSColor.White );  

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

         switch ( e.Key.KeyCode )
         {
            case RSKeyCode.Escape:
            {
               MainWindow.Quit();
               return;
            }
         }
      }

      private static void MainWindowRender( object sender, FrameEventArgs e )
      {
         MainWindow.Draw();
      }

      private static void MainWindowUpdate( object sender, FrameEventArgs e )
      {

      }
   }
}
