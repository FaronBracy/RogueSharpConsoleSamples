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

      public void Update( FrameEventArgs e )
      {
         float amount = ( (float) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = RSColor.Lerp( StartColor, EndColor, amount );
         //Console.WriteLine( $"Updating Animation at {e.TotalElapsedMs} - {CurrentColor} - {amount}" );
      }

      public void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetBackColor( X, Y, CurrentColor );
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
            Animation animation = new Animation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
            AnimationManager.AddAnimation( ++i * SpeedMs, animation );
         }
      }
   }
}
