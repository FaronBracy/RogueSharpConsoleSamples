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

   public class ActionWithDirection : IAction
   {
      protected int _dx;
      protected int _dy;
      protected Entity _entity;
      protected Engine _engine;
      protected int _destinationX;
      protected int _destinationY;
      protected Entity _target;

      public ActionWithDirection( int dx, int dy, Entity entity, Engine engine )
      {
         _dx = dx;
         _dy = dy;
         _entity = entity;
         _engine = engine;
         _destinationX = _entity.X + dx;
         _destinationY = _entity.Y + dy;
         _target = _engine.GameMap.GetBlockingEntityAtLocation( _destinationX, _destinationY );
      }

      public virtual void Perform()
      {
         throw new NotImplementedException();
      }
   }

   public class MovementAction : ActionWithDirection
   {
      public MovementAction( int dx, int dy, Entity entity, Engine engine )
         : base( dx, dy, entity, engine ) { }

      public override void Perform()
      {
         if ( !_engine.GameMap.InBounds( _destinationX, _destinationY ) )
         {
            return;
         }
         if ( !_engine.GameMap.GetCell( _destinationX, _destinationY ).IsWalkable )
         {
            return;
         }
         if ( _target != null )
         {
            return;
         }
         _entity.Move( _dx, _dy );
      }
   }

   public class MeleeAction : ActionWithDirection
   {
      public MeleeAction( int dx, int dy, Entity entity, Engine engine )
         : base( dx, dy, entity, engine ) { }

      public override void Perform()
      {
         if ( _target != null )
         {
            Console.WriteLine( $"You kick the {_target.Name}, much to its annoyance!" );
         }
      }
   }

   public class BumpAction : ActionWithDirection
   {
      public BumpAction( int dx, int dy, Entity entity, Engine engine )
         : base( dx, dy, entity, engine ) { }

      public override void Perform()
      {
         if ( _target != null )
         {
            new MeleeAction( _dx, _dy, _entity, _engine ).Perform();
         }
         else
         {
            new MovementAction( _dx, _dy, _entity, _engine ).Perform();
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
