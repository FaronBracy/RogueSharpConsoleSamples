using RogueSharp.ConsoleEngine;

namespace AutoBattler
{
   public class Animation
   {
      public RSColor EndColor { get; set; }
      public RSColor StartColor { get; set; }
      public long DurationMs { get; set; }

      public long StartTimeMs { get; set; }
      public RSColor CurrentColor { get; set; }

      public Animation( long durationMs, RSColor startColor, RSColor endColor )
      {
         DurationMs = durationMs;
         StartColor = startColor;
         EndColor = endColor;
         StartTimeMs = Game.MainWindow.Stopwatch.ElapsedMilliseconds;
      }

      public void Update( FrameEventArgs e )
      {
         double percentComplete = ( (double) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = RSColor.Blend( StartColor, EndColor, (float) percentComplete );
      }

      public void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetBackColor( 5, 5, CurrentColor );
      }
   }
}
