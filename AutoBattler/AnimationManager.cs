using RogueSharp.ConsoleEngine;

namespace AutoBattler
{
   public static class AnimationManager
   {
      public static List<Animation> Animations = new List<Animation>();

      public static void AddAnimation(long startOffsetMs, Animation animation )
      {
         animation.StartTimeMs = Game.MainWindow.Stopwatch.ElapsedMilliseconds + startOffsetMs;
         //Console.WriteLine( $"Adding Animation at {animation.StartTimeMs}" );
         Animations.Add( animation );
      }

      public static void RemoveAnimation( Animation animation )
      {
         // TODO: Expire the animation so it will remove on the next update
         Console.WriteLine( "Removing Animation" );
         Animations.Remove( animation );
      }

      public static void Update( FrameEventArgs e )
      {
         foreach ( Animation animation in Animations )
         {
            if ( animation.StartTimeMs <= Game.MainWindow.Stopwatch.ElapsedMilliseconds )
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
            if ( animation.StartTimeMs <= Game.MainWindow.Stopwatch.ElapsedMilliseconds )
            {
               animation.Render( e );
            }
         }
      }
   }
}
