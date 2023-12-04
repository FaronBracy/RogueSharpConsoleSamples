using RogueSharp.ConsoleEngine;

namespace AutoBattler
{
   public static class AnimationSystem
   {
      private static readonly List<IAnimation> Animations = new List<IAnimation>();

      public static void AddAnimation( IAnimation animation, long startOffsetMs = 0 )
      {
         animation.StartTimeMs = Game.MainWindow.ElapsedMilliseconds + startOffsetMs;
         Animations.Add( animation );
      }

      public static void Update( FrameEventArgs e )
      {
         foreach ( IAnimation animation in Animations )
         {
            if ( animation.StartTimeMs <= Game.MainWindow.ElapsedMilliseconds )
            {
               animation.Update( e );
            }
         }

         Animations.RemoveAll( anim => anim.IsComplete );
      }

      public static void Render( FrameEventArgs e )
      {
         foreach ( IAnimation animation in Animations )
         {
            if ( animation.StartTimeMs <= Game.MainWindow.ElapsedMilliseconds )
            {
               animation.Render( e );
            }
         }
      }
   }

   public static class AnimationManager
   {
      public static List<Animation> Animations = new List<Animation>();

      public static void AddAnimation(long startOffsetMs, Animation animation )
      {
         animation.StartTimeMs = Game.MainWindow.ElapsedMilliseconds + startOffsetMs;
         //Console.WriteLine( $"Adding Animation at {animation.StartTimeMs}" );
         Animations.Add( animation );
      }

      public static void RemoveAnimation( Animation animation )
      {
         // TODO: Expire the animation so it will remove on the next update
         //Console.WriteLine( "Removing Animation" );
         Animations.Remove( animation );
      }

      public static void Update( FrameEventArgs e )
      {
         foreach ( Animation animation in Animations )
         {
            if ( animation.StartTimeMs <= Game.MainWindow.ElapsedMilliseconds )
            {
               animation.Update( e );
            }
         }

         Animations.RemoveAll( animation => animation.IsComplete );
      }

      public static void Render( FrameEventArgs e )
      {
         foreach ( Animation animation in Animations )
         {
            if ( animation.StartTimeMs <= Game.MainWindow.ElapsedMilliseconds )
            {
               animation.Render( e );
            }
         }
      }
   }
}
