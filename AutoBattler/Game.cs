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
      //BitmapFont bitmapFont = new BitmapFont( 16, 24, 16, 176, "oryx_16x24_black.png", BitmapFontLayout.Cp437 );
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

      string word = "ABCDEXYZabcdexyz";
      foreach ( int i in word )
      {
         Console.WriteLine( $"{i}" );
      }

      foreach ( char c in word )
      {
         Console.WriteLine( $"{Convert.ToInt32(c)}" );
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

      MainWindow.RootConsole.SetChar( Mouse.X, Mouse.Y, '.' );
      MainWindow.RootConsole.SetColor( Mouse.X, Mouse.Y, new RSColor( 255, 255, 255 ) );
      MainWindow.RootConsole.SetBackColor( Mouse.X, Mouse.Y, new RSColor( 0, 255, 0 ) );

      MainWindow.RootConsole.SetChar( 10, 10, '\u2192' );
      MainWindow.RootConsole.SetChar( 11, 10, BitmapFont.TileIndexToUnicodeInt( 27 ) );
      AnimationManager.Render( e );
      MainWindow.Draw();
   }

   private static void MainWindowMouseDown( object? sender, MouseEventArgs e )
   {
      // TODO - Create a way to map new unicode values to tiles - Start this at 10000 for an example game
      // TODO - Rename RSCell to RSTile
      // TODO - Matrix animation effect loading screen

      Console.WriteLine( e );

      //AnimationSeries animationSeries = new AnimationSeries();

      CellAnimation animation = new CellAnimation()
         .WithDuration( 500 )
         .WithSymbolAnimation( new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' } )
         .WithColorAnimation( RSColor.Black, RSColor.White )
         .WithBackgroundColorAnimation( RSColor.Yellow, RSColor.Red );

      LineAnimation lineAnimation = new LineAnimation( new Point( 5, 5 ), new Point( Mouse.X, Mouse.Y ), animation, 50 );
      AnimationGroup lineAnimationGroup = lineAnimation.Generate();
      //LineAnimation.Begin();


      CircleAnimation circleAnimation = new CircleAnimation( new Point( Mouse.X, Mouse.Y ), 5, animation, 50 );
      AnimationGroup circleAnimationGroup = circleAnimation.Generate();
      //circleAnimation.Begin();

      AnimationManager.AddAnimations( lineAnimationGroup.GetAnimations() );
      AnimationManager.AddAnimations( circleAnimationGroup.GetAnimations(), lineAnimationGroup.AnimationGroupLengthMs );
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