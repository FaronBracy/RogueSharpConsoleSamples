using RogueSharp.ConsoleEngine;

namespace AutoBattler
{
   public static class AnimationManager
   {
      public static List<Animation> Animations = new List<Animation>();

      public static void AddAnimation( Animation animation )
      {
         Console.WriteLine( "Adding Animation" );
         animation.StartTimeMs = Game.MainWindow.Stopwatch.ElapsedMilliseconds;
         Animations.Add( animation );
      }

      public static void RemoveAnimation( Animation animation )
      {
         Console.WriteLine( "Removing Animation" );
         Animations.Remove( animation );
      }

      public static void Update( FrameEventArgs e )
      {
         foreach ( Animation animation in Animations )
         {
            animation.Update( e );
         }

         Animations.RemoveAll( animation => animation.IsComplete );
      }

      public static void Render( FrameEventArgs e )
      {
         foreach ( Animation animation in Animations )
         {
            animation.Render( e );
         }
      }
   }
}
