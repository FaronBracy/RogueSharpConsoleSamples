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

      for ( int x = 5; x < 50; x++ )
      {
         Animation animation = new Animation( 1000, new RSColor(1, 0, 0), new RSColor(255,0,0), x, 5 );
         AnimationManager.AddAnimation( (x - 4) * 10, animation );
      }
       

      MainWindow.Start();
   }

   private static void MainWindowUpdate( object? sender, FrameEventArgs e )
   {
      AnimationManager.Update( e );
   }

   private static void MainWindowRender( object? sender, FrameEventArgs e )
   {
      MainWindow.RootConsole.Clear();
      MainWindow.RootConsole.SetChar( 5, 5, '@' );
      AnimationManager.Render( e );
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