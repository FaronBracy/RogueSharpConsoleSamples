using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class InputHandler
   {
      public IAction HandleKey( RSKey key )
      {
         switch( key.KeyCode )
         {
            case RSKeyCode.Escape:
            {
               return new EscapeAction( Game.MainWindow );
            }
            case RSKeyCode.Up:
            {
               return new MovementAction( 0, -1, Game.Player );
            }
            case RSKeyCode.Down:
            {
               return new MovementAction( 0, 1, Game.Player );
            }
            case RSKeyCode.Left:
            {
               return new MovementAction( -1, 0, Game.Player );
            }
            case RSKeyCode.Right:
            {
               return new MovementAction( 1, 0, Game.Player );
            }
         }

         return new NoAction();
      }
   }
}
