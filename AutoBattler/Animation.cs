using RogueSharp;
using RogueSharp.ConsoleEngine;
using Point = RogueSharp.Point;

namespace AutoBattler
{
   public interface IAnimation
   {
      void Update( FrameEventArgs e );
      void Render( FrameEventArgs e );
      long StartTimeMs { get; set; }
      bool IsComplete { get; }
   }

   public class CellAnimation : IAnimation
   {
      public int X { get; set; }
      public int Y { get; set; }
      public long StartTimeMs { get; set; }
      public long DurationMs { get; set; }
      public long EndTimeMs => StartTimeMs + DurationMs;
      public long LastUpdateMs { get; private set; }
      public long LastRenderMs { get; private set; }

      public bool IsComplete => LastUpdateMs - StartTimeMs > DurationMs;

      public RSColor? StartBackgroundColor { get; set; }
      public RSColor? EndBackgroundColor { get; set; }
      public RSColor? CurrentBackgroundColor { get; private set; }

      public RSColor? StartColor { get; set; }
      public RSColor? EndColor { get; set; }
      public RSColor? CurrentColor { get; private set; }

      public char? StartSymbol { get; set; }
      public char? EndSymbol { get; set; }
      public char? CurrentSymbol { get; private set; }

      public CellAnimation()
      {
      }

      public CellAnimation( int x, int y, long durationMs )
      {
         X = x;
         Y = y;
         DurationMs = durationMs;
      }

      public CellAnimation At( int x, int y )
      {
         X = x;
         Y = y;
         return this;
      }

      public CellAnimation WithDuration( long durationMs )
      {
         DurationMs = durationMs;
         return this;
      }

      public CellAnimation WithBackgroundColorAnimation( RSColor startColor, RSColor endColor )
      {
         StartBackgroundColor = startColor;
         EndBackgroundColor = endColor;
         return this;
      }

      public CellAnimation WithColorAnimation( RSColor startColor, RSColor endColor )
      {
         StartColor = startColor;
         EndColor = endColor;
         return this;
      }

      public CellAnimation WithSymbolAnimation( char startSymbol, char endSymbol )
      {
         StartSymbol = startSymbol;
         EndSymbol = endSymbol;
         return this;
      }

      public void Update( FrameEventArgs e )
      {
         float amount = ( (float) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         if ( StartBackgroundColor.HasValue && EndBackgroundColor.HasValue )
         {
            // Console.WriteLine( $"{e.TotalElapsedMs} - BG Animation - Start: {StartBackgroundColor.Value} End: {EndBackgroundColor.Value} Amount: {amount}" );
            CurrentBackgroundColor = RSColor.Lerp( StartBackgroundColor.Value, EndBackgroundColor.Value, amount );
         }
         if ( StartColor.HasValue && EndColor.HasValue )
         {
            CurrentColor = RSColor.Lerp( StartColor.Value, EndColor.Value, amount );
         }
         if ( StartSymbol.HasValue && EndSymbol.HasValue )
         {
            CurrentSymbol = (char) ( StartSymbol.Value + ( EndSymbol.Value - StartSymbol.Value ) * amount );
         }
         LastUpdateMs = e.TotalElapsedMs;
      }

      public void Render( FrameEventArgs e )
      {
         if ( CurrentBackgroundColor.HasValue )
         {
            // Console.WriteLine( $"{e.TotalElapsedMs} - Rendering BG Color: {CurrentBackgroundColor.Value}" );
            Game.MainWindow.RootConsole.SetBackColor( X, Y, CurrentBackgroundColor.Value );
         }
         if ( CurrentColor.HasValue )
         {
            Game.MainWindow.RootConsole.SetColor( X, Y, CurrentColor.Value );
         }
         LastRenderMs = e.TotalElapsedMs;
      }

      public CellAnimation Clone()
      {
         // Make a deep copy of the animation
         return new CellAnimation
         {
            X = X,
            Y = Y,
            StartTimeMs = StartTimeMs,
            DurationMs = DurationMs,
            LastUpdateMs = LastUpdateMs,
            LastRenderMs = LastRenderMs,
            StartBackgroundColor = StartBackgroundColor,
            EndBackgroundColor = EndBackgroundColor,
            CurrentBackgroundColor = CurrentBackgroundColor,
            StartColor = StartColor,
            EndColor = EndColor,
            CurrentColor = CurrentColor,
            StartSymbol = StartSymbol,
            EndSymbol = EndSymbol,
            CurrentSymbol = CurrentSymbol
         };
      }
   }

   public class AnimationSeries : IAnimation
   {
      private readonly List<CellAnimation> _animations = new List<CellAnimation>();
      private int _currentAnimationIndex;

      public long StartTimeMs
      {
         get => _animations[0].StartTimeMs;
         set => _animations[0].StartTimeMs = value;
      }

      public bool IsComplete => _animations.All( a => a.IsComplete );
      public long EndTimeMs => _animations.Max( a => a.EndTimeMs );

      public void Add( CellAnimation animation )
      {
         _animations.Add( animation );
      }

      public void Update( FrameEventArgs e )
      {
         if ( IsComplete )
         {
            // Fire an event to notify that the animation series is complete
            return;
         }

         if ( _animations[_currentAnimationIndex].IsComplete )
         {
            // Advance to the next frame
            _currentAnimationIndex++;
            if ( _currentAnimationIndex >= _animations.Count )
            {
               return;
            }
            _animations[_currentAnimationIndex].StartTimeMs = e.TotalElapsedMs;
         }

         // Console.WriteLine( $"{e.TotalElapsedMs} - Updating animation {_currentAnimationIndex} of {_animations.Count}" );
         _animations[_currentAnimationIndex].Update( e );
      }

      public void Render( FrameEventArgs e )
      {
         if ( IsComplete )
         {
            return;
         }

         if ( _currentAnimationIndex >= _animations.Count )
         {
            return;
         }
         // Console.WriteLine( $"{e.TotalElapsedMs} - Rendering animation {_currentAnimationIndex} of {_animations.Count}" );
         _animations[_currentAnimationIndex].Render( e );
      }
   }


   public class AnimationGroup
   {
      public long AnimationGroupLengthMs { get; private set; }

      private readonly List<(CellAnimation, long)> _animations = new List<(CellAnimation, long)>();

      public void Add( CellAnimation cellAnimation, long startTimeOffsetMs )
      {
         _animations.Add( (cellAnimation, startTimeOffsetMs) );

         AnimationGroupLengthMs = Math.Max( AnimationGroupLengthMs, startTimeOffsetMs + cellAnimation.DurationMs );
      }

      public List<(CellAnimation, long)> GetAnimations()
      {
         return _animations;
      }
   }

   public class LineAnimation2
   {
      public Point Origin { get; set; }
      public Point Destination { get; set; }
      public CellAnimation CellAnimation { get; set; }
      public long SpeedMs { get; set; }

      public LineAnimation2( Point origin, Point destination, CellAnimation cellAnimation, long speedMs )
      {
         Origin = origin;
         Destination = destination;
         CellAnimation = cellAnimation;
         SpeedMs = speedMs;
      }

      public void Begin()
      {
         int i = 0;
         foreach ( Cell cell in Game.Map.GetCellsAlongLine( Origin.X, Origin.Y, Destination.X, Destination.Y ) )
         {
            CellAnimation cellAnimation = CellAnimation.Clone().At( cell.X, cell.Y );
            AnimationSystem.AddAnimation( cellAnimation, ++i * SpeedMs );
         }
      }

      public AnimationGroup Generate()
      {
         AnimationGroup animations = new AnimationGroup();
         int i = 0;
         foreach ( Cell cell in Game.Map.GetCellsAlongLine( Origin.X, Origin.Y, Destination.X, Destination.Y ) )
         {
            CellAnimation cellAnimation = CellAnimation.Clone().At( cell.X, cell.Y );
            animations.Add( cellAnimation, ++i * SpeedMs );
         }
         return animations;
      }
   }

   public class CircleAnimation2
   {
      public Point Center { get; set; }
      public int Radius { get; set; }
      public CellAnimation CellAnimation { get; set; }
      public long SpeedMs { get; set; }

      public CircleAnimation2( Point center, int radius, CellAnimation cellAnimation, long speedMs )
      {
         Center = center;
         CellAnimation = cellAnimation;
         SpeedMs = speedMs;
         Radius = radius;
      }

      public void Begin( long startOffsetMs = 0 )
      {
         for ( int i = 1; i <= Radius; i++ )
         {
            foreach ( Cell cell in Game.Map.GetBorderCellsInCircle( Center.X, Center.Y, i ) )
            {
               CellAnimation cellAnimation = CellAnimation.Clone().At( cell.X, cell.Y );
               AnimationSystem.AddAnimation( cellAnimation, ( i * SpeedMs ) + startOffsetMs );
            }
         }
      }

      public AnimationGroup Generate()
      {
         AnimationGroup animations = new AnimationGroup();

         List<Cell>[] circleCellsArray = new List<Cell>[Radius];
         HashSet<Cell> usedCells = new HashSet<Cell>();
         for ( int i = 1; i <= Radius; i++ )
         {
            circleCellsArray[i - 1] = new List<Cell>();

            foreach ( Cell cell in Game.Map.GetCellsInCircle( Center.X, Center.Y, i ) )
            {
               if ( !usedCells.Contains( cell ) )
               {
                  circleCellsArray[i - 1].Add( cell );
                  usedCells.Add( cell );
               }
            }
         }

         for ( int i = 0; i < circleCellsArray.Length; i++ )
         {
            foreach ( Cell cell in circleCellsArray[i] )
            {
               CellAnimation cellAnimation = CellAnimation.Clone().At( cell.X, cell.Y );
               animations.Add( cellAnimation, i * SpeedMs );
            }
         }
         
         return animations;
      }
   }


   public class Animation
   {
      public RSColor EndColor { get; set; }
      public RSColor StartColor { get; set; }
      public long DurationMs { get; set; }
      public int X { get; set; }
      public int Y { get; set; }

      public long StartTimeMs { get; set; }
      public RSColor CurrentColor { get; set; }
      public bool IsComplete => Game.MainWindow.ElapsedMilliseconds - StartTimeMs > DurationMs;

      public Animation( long durationMs, RSColor startColor, RSColor endColor, int x, int y )
      {
         DurationMs = durationMs;
         StartColor = startColor;
         EndColor = endColor;
         X = x;
         Y = y;
      }

      public virtual void Update( FrameEventArgs e )
      {
      }

      public virtual void Render( FrameEventArgs e )
      {
      }
   }

   public class BackgroundAnimation : Animation
   {
      public BackgroundAnimation( long durationMs, RSColor startColor, RSColor endColor, int x, int y ) : base(
         durationMs, startColor, endColor, x, y )
      {
      }

      public override void Update( FrameEventArgs e )
      {
         float amount = ( (float) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = StartColor;
         CurrentColor = RSColor.Lerp( StartColor, EndColor, amount );
         //Console.WriteLine( $"Updating Animation at {e.TotalElapsedMs} - {CurrentColor} - {amount}" );
      }
      public override void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetBackColor( X, Y, CurrentColor );
      }
   }

   public class SymbolAnimation : Animation
   {
      public char StartSymbol { get; set; }
      public char EndSymbol { get; set; }

      public SymbolAnimation( long durationMs, char startSymbol, char endSymbol, RSColor startColor, RSColor endColor,
         int x, int y ) : base( durationMs, startColor, endColor, x, y )
      {
         StartSymbol = startSymbol;
         EndSymbol = endSymbol;
      }

      public override void Update( FrameEventArgs e )
      {
         float amount = ( (float) e.TotalElapsedMs - StartTimeMs ) / DurationMs;
         CurrentColor = RSColor.Lerp( StartColor, EndColor, amount );
         //Console.WriteLine( $"Updating Animation at {e.TotalElapsedMs} - {CurrentColor} - {amount}" );
      }

      public override void Render( FrameEventArgs e )
      {
         Game.MainWindow.RootConsole.SetChar( X, Y, StartSymbol );
         Game.MainWindow.RootConsole.SetColor( X, Y, CurrentColor );
      }
   }

   public class LineAnimation
   {
      public long DurationMs { get; set; }
      public int SpeedMs { get; set; }
      public Point Start { get; set; }
      public Point End { get; set; }
      public RSColor StartColor { get; set; }
      public RSColor EndColor { get; set; }
      public char Symbol { get; set; }

      public LineAnimation( long durationMs, int speedMs, Point start, Point end, RSColor startColor, RSColor endColor, char symbol )
      {
         DurationMs = durationMs;
         SpeedMs = speedMs;
         Start = start;
         End = end;
         StartColor = startColor;
         EndColor = endColor;
         Symbol = symbol;
      }

      public void Begin()
      {
         int i = 0;
         foreach ( Cell cell in Game.Map.GetCellsAlongLine( Start.X, Start.Y, End.X, End.Y ) )
         {
            //Console.WriteLine( $"{cell.X},{cell.Y}" );
            BackgroundAnimation animation = new BackgroundAnimation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
            AnimationManager.AddAnimation( ++i * SpeedMs, animation );
            SymbolAnimation symbolAnimation = new SymbolAnimation( SpeedMs, Symbol, Symbol, RSColor.White, RSColor.White, cell.X, cell.Y );
            AnimationManager.AddAnimation( i * SpeedMs, symbolAnimation );
         }
      }
   }

   public class CircleAnimation
   {
      public long DurationMs { get; set; }
      public int SpeedMs { get; set; }
      public Point Start { get; set; }
      public int Radius { get; set; }
      public RSColor StartColor { get; set; }
      public RSColor EndColor { get; set; }

      public CircleAnimation( long durationMs, int speedMs, Point start, int radius, RSColor startColor, RSColor endColor )
      {
         DurationMs = durationMs;
         SpeedMs = speedMs;
         Start = start;
         Radius = radius;
         StartColor = startColor;
         EndColor = endColor;
      }

      public void Begin()
      {
         for ( int i = 1; i <= Radius; i++ )
         {
            foreach ( Cell cell in Game.Map.GetBorderCellsInCircle( Start.X, Start.Y, i ) )
            {
               BackgroundAnimation animation = new BackgroundAnimation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
               AnimationManager.AddAnimation( i * SpeedMs, animation );
            }
         }
      }
      public void BeginSolid()
      {
         List<Cell>[] circleCellsArray = new List<Cell>[Radius];
         HashSet<Cell> usedCells = new HashSet<Cell>();
         for ( int i = 1; i <= Radius; i++ )
         {
            circleCellsArray[i - 1] = new List<Cell>();

            foreach ( Cell cell in Game.Map.GetCellsInCircle( Start.X, Start.Y, i ) )
            {
               if ( !usedCells.Contains( cell ) )
               {
                  circleCellsArray[i - 1].Add( cell );
                  usedCells.Add( cell );
               }
            }
         }

         for ( int i = 0; i < circleCellsArray.Length; i++ )
         {
            foreach ( Cell cell in circleCellsArray[i] )
            {
               BackgroundAnimation animation = new BackgroundAnimation( DurationMs, StartColor, EndColor, cell.X, cell.Y );
               AnimationManager.AddAnimation( i * SpeedMs, animation );
            }
         }
      }
   }

   public static class Effects
   {
      public static void ShootArrow( Point start, Point end )
      {
         LineAnimation lineAnimation = new LineAnimation( 50, 10, start, end, RSColor.Green, RSColor.LightGreen, '\u2192' );
         lineAnimation.Begin();
      }

      public static void Explosion( Point center, int radius )
      {
         CircleAnimation circleAnimation = new CircleAnimation( 1000, 50, center
            , radius, RSColor.Yellow, new RSColor( 255, 0, 0 ) );
         circleAnimation.BeginSolid();
      }
   }
}
