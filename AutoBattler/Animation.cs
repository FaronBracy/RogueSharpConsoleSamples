using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace AutoBattler
{
   public class Animation
   {
      public RSColor EndColor { get; set; }
      public RSColor StartColor { get; set; }
      public long DurationMs { get; set; }
      public int X { get; set; }
      public int Y { get; set; }

      public long StartTimeMs { get; set; }
      public RSColor CurrentColor { get; set; }
      public bool IsComplete => Game.MainWindow.Stopwatch.ElapsedMilliseconds - StartTimeMs > DurationMs;

      public Animation( long durationMs, RSColor startColor, RSColor endColor, int x, int y )
      {
         DurationMs = durationMs;
         StartColor = startColor;
         EndColor = endColor;
         X = x;
         Y = y;
      }

      public virtual void Update( FrameEventArgs e )
      {
      }
      
      public virtual void Render( FrameEventArgs e )
      {
      }
   }

   public class BackgroundAnimation : Animation
   {
      public BackgroundAnimation( long durationMs, RSColor startColor, RSColor endColor, int x, int y ) : base(
         durationMs, startColor, endColor, x, y )
      {
      }

      public override void Update( FrameEventArgs e )
      {
         float amount = ( (float) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = StartColor;
         CurrentColor = RSColor.Lerp( StartColor, EndColor, amount );
         //Console.WriteLine( $"Updating Animation at {e.TotalElapsedMs} - {CurrentColor} - {amount}" );
      }
      public override void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetBackColor( X, Y, CurrentColor );
      }
   }

   public class SymbolAnimation : Animation
   {
      public char StartSymbol { get; set; }
      public char EndSymbol { get; set; }

      public SymbolAnimation( long durationMs, char startSymbol, char endSymbol, RSColor startColor, RSColor endColor,
         int x, int y ) : base( durationMs, startColor, endColor, x, y )
      {
         StartSymbol = startSymbol;
         EndSymbol = endSymbol;
      }

      public override void Update( FrameEventArgs e )
      {
         float amount = ( (float) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = RSColor.Lerp( StartColor, EndColor, amount );
         //Console.WriteLine( $"Updating Animation at {e.TotalElapsedMs} - {CurrentColor} - {amount}" );
      }

      public override void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetChar( X, Y, StartSymbol );
         Game.MainWindow.RootConsole.SetColor( X, Y, CurrentColor );
      }
   }

   public class LineAnimation
   {
      public long DurationMs { get; set; }
      public int SpeedMs { get; set; }
      public Point Start { get; set; }
      public Point End { get; set; }
      public RSColor StartColor { get; set; }
      public RSColor EndColor { get; set; }
      public char Symbol { get; set; }

      public LineAnimation( long durationMs, int speedMs, Point start, Point end, RSColor startColor, RSColor endColor, char symbol )
      {
         DurationMs = durationMs;
         SpeedMs = speedMs;
         Start = start;
         End = end;
         StartColor = startColor;
         EndColor = endColor;
         Symbol = symbol;
      }

      public void Begin()
      {
         int i = 0;
         foreach ( Cell cell in Game.Map.GetCellsAlongLine( Start.X, Start.Y, End.X, End.Y ) )
         {
            //Console.WriteLine( $"{cell.X},{cell.Y}" );
            BackgroundAnimation animation = new BackgroundAnimation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
            AnimationManager.AddAnimation( ++i * SpeedMs, animation );
            SymbolAnimation symbolAnimation = new SymbolAnimation( SpeedMs, Symbol, Symbol, RSColor.White, RSColor.White, cell.X, cell.Y );
            AnimationManager.AddAnimation( i * SpeedMs, symbolAnimation );
         }
      }
   }

   public class CircleAnimation
   {
      public long DurationMs { get; set; }
      public int SpeedMs { get; set; }
      public Point Start { get; set; }
      public int Radius { get; set; }
      public RSColor StartColor { get; set; }
      public RSColor EndColor { get; set; }

      public CircleAnimation( long durationMs, int speedMs, Point start, int radius, RSColor startColor, RSColor endColor )
      {
         DurationMs = durationMs;
         SpeedMs = speedMs;
         Start = start;
         Radius = radius;
         StartColor = startColor;
         EndColor = endColor;
      }

      public void Begin()
      {
         for ( int i = 1; i <= Radius; i++ )
         {
            foreach ( Cell cell in Game.Map.GetBorderCellsInCircle( Start.X, Start.Y, i ) )
            {
               BackgroundAnimation animation = new BackgroundAnimation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
               AnimationManager.AddAnimation( i * SpeedMs, animation );
            }
         }
      }
      public void BeginSolid()
      {
         List<Cell>[] circleCellsArray = new List<Cell>[Radius];
         HashSet<Cell> usedCells = new HashSet<Cell>();
         for ( int i = 1; i <= Radius; i++ )
         {
            circleCellsArray[i - 1] = new List<Cell>();

            foreach ( Cell cell in Game.Map.GetCellsInCircle( Start.X, Start.Y, i ) )
            {
               if ( !usedCells.Contains( cell ) )
               {
                  circleCellsArray[i - 1].Add( cell );
                  usedCells.Add( cell );
               }
            }
         }

         for ( int i = 0; i < circleCellsArray.Length; i++ )
         {
            foreach ( Cell cell in circleCellsArray[i] )
            {
               BackgroundAnimation animation = new BackgroundAnimation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
               AnimationManager.AddAnimation( i * SpeedMs, animation );
            }
         }
      }
   }

   public static class Effects
   {
      public static void ShootArrow( Point start, Point end )
      {
         LineAnimation lineAnimation = new LineAnimation( 50, 10, start, end, RSColor.Green, RSColor.LightGreen, '\u2192' );
         lineAnimation.Begin();
      }
   }
}
