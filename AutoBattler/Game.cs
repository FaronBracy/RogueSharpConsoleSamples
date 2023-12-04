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
      //AnimationManager.Update( e );
      AnimationSystem.Update( e );
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
      //AnimationManager.Render( e );
      AnimationSystem.Render( e );
      MainWindow.Draw();
   }

   private static void MainWindowMouseDown( object? sender, MouseEventArgs e )
   {
      // TODO - Create a way to map new unicode values to tiles - Start this at 10000 for an example game
      // TODO - Rename RSCell to RSTile
      // TODO - Animation chaining
      // TODO - Matrix animation effect loading screen
      // TODO - Get symbol animation working

      Console.WriteLine( e );
      //Effects.ShootArrow( new Point( 5, 5 ), new Point( Mouse.X, Mouse.Y ) );
      //Effects.Explosion( new Point( Mouse.X, Mouse.Y ), 7 );


      //AnimationSeries animationSeries = new AnimationSeries();

      CellAnimation animation = new CellAnimation()
         .WithDuration( 250 )
         .WithBackgroundColorAnimation( RSColor.Yellow, RSColor.Red );

      LineAnimation2 lineAnimation2 = new LineAnimation2( new Point( 5, 5 ), new Point( Mouse.X, Mouse.Y ), animation, 50 );
      AnimationGroup lineAnimationGroup = lineAnimation2.Generate();
      //LineAnimation2.Begin();
      

      CircleAnimation2 circleAnimation2 = new CircleAnimation2( new Point( Mouse.X, Mouse.Y ), 5, animation, 50 );
      AnimationGroup circleAnimationGroup = circleAnimation2.Generate();
      //circleAnimation2.Begin();

      AnimationSystem.AddAnimations( lineAnimationGroup.GetAnimations() );
      AnimationSystem.AddAnimations( circleAnimationGroup.GetAnimations(), lineAnimationGroup.AnimationGroupLengthMs );

      //CellAnimation cellAnimation = new CellAnimation()
      //   .At( Mouse.X, Mouse.Y )
      //   .WithDuration( 2500 )
      //   .WithBackgroundColorAnimation( RSColor.Yellow, RSColor.Red );

      //AnimationSystem.AddAnimation( cellAnimation );

      //CircleAnimation circleAnimation = new CircleAnimation( 1000, 50, new Point( Mouse.X, Mouse.Y )
      //   , 5, RSColor.Yellow, new RSColor( 255, 0, 0 ) );
      //circleAnimation.BeginSolid();
      //LineAnimation lineAnimation = new LineAnimation( 1000, 10, new Point( 5, 5 ), new Point( Mouse.X, Mouse.Y )
      //   , new RSColor( 255, 0, 0 ), new RSColor( 1, 0, 0 ), '*' );
      //lineAnimation.Begin();
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