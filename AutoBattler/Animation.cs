﻿using RogueSharp.ConsoleEngine;

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
         double percentComplete = ( (double) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = RSColor.Blend( StartColor, EndColor, (float) percentComplete );
      }

      public void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetBackColor( X, Y, CurrentColor );
      }
   }
}
