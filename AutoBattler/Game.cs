using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace AutoBattler;

public static class Game
{
   public static Map Map { get; private set; }
   public static RSWindow MainWindow { get; private set; }
   public static RSMouse Mouse { get; private set; }

   public static void Main()
   {
      BitmapFont bitmapFont = new BitmapFont( 10, 10, 16, 16, "qbicfeet_10x10.png", BitmapFontLayout.Cp437 );
      Mouse = new RSMouse();

      MainWindow = new RSWindow( bitmapFont, 100, 50, "Auto Battler" );
      MainWindow.Update += MainWindowUpdate;
      MainWindow.Render += MainWindowRender;
      MainWindow.MouseDown += MainWindowMouseDown;
      MainWindow.MouseMove += MainWindowMouseMove;
      MainWindow.KeyDown += MainWindowKeyDown;
      MainWindow.Quitting += MainWindowQuitting;

      Map = new Map( 100, 50 );
      foreach ( Cell cell in Map.GetAllCells() )
      {
         Map.SetCellProperties( cell.X, cell.Y, true, true );
      }
      

      //for ( int x = 5; x < 50; x++ )
      //{
      //   Animation animation = new Animation( 1000, new RSColor(1, 0, 0), new RSColor(255,0,0), x, 5 );
      //   AnimationManager.AddAnimation( (x - 4) * 10, animation );
      //}
       

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
      MainWindow.RootConsole.SetChar( Mouse.X, Mouse.Y, '.' );
      MainWindow.RootConsole.SetColor( Mouse.X, Mouse.Y, new RSColor( 255, 0, 0 ) );
      MainWindow.RootConsole.SetBackColor( Mouse.X, Mouse.Y, new RSColor( 0, 0, 255 ) );
      AnimationManager.Render( e );
      MainWindow.Draw();
   }

   private static void MainWindowMouseDown( object? sender, MouseEventArgs e )
   {
      Console.WriteLine( e );
      LineAnimation lineAnimation = new LineAnimation( 1000, 10, new Point( 5, 5 ), new Point( Mouse.X, Mouse.Y )
         , new RSColor( 255, 0, 0 ), new RSColor( 1, 0, 0 ), '*' );
      lineAnimation.Begin();
   }

   private static void MainWindowMouseMove( object? sender, MouseEventArgs e )
   {
      Mouse = e.Mouse;
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