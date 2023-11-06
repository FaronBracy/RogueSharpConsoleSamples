using RogueSharp.ConsoleEngine;

namespace AutoBattler;

public static class Game
{
   public static RSWindow MainWindow { get; private set; }

   public static void Main()
   {
      BitmapFont bitmapFont = new BitmapFont( 10, 10, 16, 16, "qbicfeet_10x10.png", BitmapFontLayout.Cp437 );
      MainWindow = new RSWindow( bitmapFont, 100, 50, "Auto Battler" );
      MainWindow.Update += MainWindowUpdate;
      MainWindow.Render += MainWindowRender;
      MainWindow.KeyDown += MainWindowKeyDown;
      MainWindow.Quitting += MainWindowQuitting;
      MainWindow.Start();
   }

   private static void MainWindowUpdate( object? sender, FrameEventArgs e )
   {
   }

   private static void MainWindowRender( object? sender, FrameEventArgs e )
   {
      MainWindow.RootConsole.Clear();
      MainWindow.RootConsole.SetChar( 5, 5, '@' );
      MainWindow.Draw();
   }

   private static void MainWindowKeyDown( object? sender, KeyEventArgs e )
   {
      if ( e.Key.KeyCode == RSKeyCode.Escape )
      {
         MainWindow.Quit();
      }
   }

   private static void MainWindowQuitting( object? sender, EventArgs e )
   {
   }
}