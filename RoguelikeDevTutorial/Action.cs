using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public interface IAction
   {
      void Perform();
   }

   public class EscapeAction : IAction
   {
      private readonly RSWindow _gameWindow;

      public EscapeAction( RSWindow gameWindow )
      {
         _gameWindow = gameWindow;
      }

      public void Perform()
      {
         _gameWindow.Quit();
      }
   }

   public abstract class ActionWithDirection : IAction
   {
      protected int _dx;
      protected int _dy;

      public abstract void Perform();
   }


   public class MovementAction : ActionWithDirection
   {
      private readonly Entity _entity;
      private readonly Engine _engine;

      public MovementAction( int dx, int dy, Entity entity, Engine engine )
      {
         _dx = dx;
         _dy = dy;
         _entity = entity;
         _engine = engine;
      }

      public override void Perform()
      {
         int destinationX = _entity.X + _dx;
         int destinationY = _entity.Y + _dy;

         if ( !_engine.GameMap.InBounds( destinationX, destinationY ) )
         {
            return;
         }
         if ( !_engine.GameMap.GetCell( destinationX, destinationY ).IsWalkable )
         {
            return;
         }
         if ( _engine.GameMap.GetBlockingEntityAtLocation( destinationX, destinationY ) != null )
         {
            return;
         }
         _entity.Move( _dx, _dy );  
      }
   }

   public class NoAction : IAction
   {
      public void Perform()
      {
      }
   }
}
