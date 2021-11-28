﻿using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class InputHandler
   {
      public IAction HandleKey( RSKey key, Engine engine )
      {
         Entity player = engine.Player;

         switch ( key.KeyCode )
         {
            case RSKeyCode.Escape:
            {
               return new EscapeAction( Game.MainWindow );
            }
            case RSKeyCode.Up:
            {
               return new MovementAction( 0, -1, player, engine );
            }
            case RSKeyCode.Down:
            {
               return new MovementAction( 0, 1, player, engine );
            }
            case RSKeyCode.Left:
            {
               return new MovementAction( -1, 0, player, engine );
            }
            case RSKeyCode.Right:
            {
               return new MovementAction( 1, 0, player, engine );
            }
         }

         return new NoAction();
      }
   }
}
