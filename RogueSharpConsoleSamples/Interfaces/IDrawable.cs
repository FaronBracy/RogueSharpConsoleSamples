﻿using RogueSharp.ConsoleEngine;
using RogueSharpRLNetSamples.Core;

namespace RogueSharpRLNetSamples.Interfaces
{
   public interface IDrawable
   {
      RLColor Color { get; set; }
      char Symbol { get; set; }
      int X { get; set; }
      int Y { get; set; }

      void Draw( RSConsole console, DungeonMap map );
   }
}