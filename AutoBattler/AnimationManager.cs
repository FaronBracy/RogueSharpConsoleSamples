using RogueSharp.ConsoleEngine;

namespace AutoBattler
{
   public static class AnimationManager
   {
      private static readonly List<IAnimation> Animations = new List<IAnimation>();

      public static void AddAnimation( IAnimation animation, long startOffsetMs = 0 )
      {
         animation.StartTimeMs = Game.MainWindow.ElapsedMilliseconds + startOffsetMs;
         Animations.Add( animation );
      }

      public static void AddAnimations( List<(CellAnimation, long)> animations, long seriesStartOffsetMs = 0 )
      {
         foreach ( ( IAnimation animation, long individualAnimationStartOffsetMs ) in animations )
         {
            AddAnimation( animation, seriesStartOffsetMs + individualAnimationStartOffsetMs );
         }
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
}
