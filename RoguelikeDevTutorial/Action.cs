using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public interface IAction
   {
      void Execute();
   }

   public class EscapeAction : IAction
   {
      private readonly RSWindow _gameWindow;

      public EscapeAction( RSWindow gameWindow )
      {
         _gameWindow = gameWindow;
      }

      public void Execute()
      {
         _gameWindow.Quit();
      }
   }

   public class MovementAction : IAction
   {
      private readonly int _dx;
      private readonly int _dy;
      private readonly Actor _actor;

      public MovementAction( int dx, int dy, Actor actor )
      {
         _dx = dx;
         _dy = dy;
         _actor = actor;
      }

      public void Execute()
      {
         _actor.Move( _dx, _dy );  
      }
   }
}
