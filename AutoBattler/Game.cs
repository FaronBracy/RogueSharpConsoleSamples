using RogueSharp.ConsoleEngine;

namespace AutoBattler;

public static class Game
{
   public static RSWindow MainWindow { get; private set; }

   public static void Main()
   {
      BitmapFont bitmapFont = new BitmapFont( 10, 10, 16, 16, "qbicfeet_10x10.png", BitmapFontLayout.Cp437 );
      MainWindow = new RSWindow( bitmapFont, 100, 50, "Auto Battler" );
      MainWindow.Render += MainWindowRender;
      MainWindow.Start();
      Console.WriteLine( "Hello World!" );
   }

   private static void MainWindowRender( object? sender, FrameEventArgs e )
   {
      MainWindow.RootConsole.Clear();
      MainWindow.RootConsole.SetChar( 5, 5, '@' );
      MainWindow.Draw();
   }
}