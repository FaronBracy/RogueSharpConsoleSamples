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
      private readonly Entity _entity;

      public MovementAction( int dx, int dy, Entity entity )
      {
         _dx = dx;
         _dy = dy;
         _entity = entity;
      }

      public void Execute()
      {
         _entity.Move( _dx, _dy );  
      }
   }

   public class NoAction : IAction
   {
      public void Execute()
      {
      }
   }
}
