using System;
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
      protected Entity _entity;
      protected Engine _engine;
      protected int _dx;
      protected int _dy;

      public abstract void Perform();
   }
   
   public class MovementAction : ActionWithDirection
   {
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

   public class MeleeAction : ActionWithDirection
   {
      public MeleeAction( int dx, int dy, Entity entity, Engine engine )
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
         Entity target = _engine.GameMap.GetBlockingEntityAtLocation( destinationX, destinationY );
         if ( target != null )
         {
            Console.WriteLine( $"You kick the {target.Name}, much to its annoyance!" );
         }
      }
   }

   public class NoAction : IAction
   {
      public void Perform()
      {
      }
   }
}
