using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class InputHandler
   {
      public IAction HandleKey( RSKey key )
      {
         Entity player = Game.Engine.Player;

         switch ( key.KeyCode )
         {
            case RSKeyCode.Escape:
            {
               return new EscapeAction( Game.MainWindow );
            }
            case RSKeyCode.Up:
            {
               return new MovementAction( 0, -1, player );
            }
            case RSKeyCode.Down:
            {
               return new MovementAction( 0, 1, player );
            }
            case RSKeyCode.Left:
            {
               return new MovementAction( -1, 0, player );
            }
            case RSKeyCode.Right:
            {
               return new MovementAction( 1, 0, player );
            }
         }

         return new NoAction();
      }
   }
}
