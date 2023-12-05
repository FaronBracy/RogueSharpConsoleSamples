using RogueSharp;
using RogueSharp.ConsoleEngine;
using Point = RogueSharp.Point;

namespace AutoBattler
{
   public class CellAnimation
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

      public char[]? Symbols { get; set; }
      public char? CurrentSymbol { get; private set; }
      public int CurrentSymbolIndex { get; private set; }

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

      public CellAnimation WithSymbolAnimation( char[] symbols )
      {
         Symbols = symbols;
         CurrentSymbol = symbols[0];
         CurrentSymbolIndex = 0;
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
         if ( Symbols != null && Symbols.Length > 0 )
         {
            CurrentSymbolIndex = Lerp( 0, Symbols.Length - 1, amount );
            CurrentSymbol = Symbols[CurrentSymbolIndex];
         }
         LastUpdateMs = e.TotalElapsedMs;
      }

      private static int Lerp( int start, int end, float amount )
      {
         int value = (int) ( start + ( ( end + 1 - start ) * amount ) );
         return Math.Clamp( value, start, end );
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
         if ( CurrentSymbol.HasValue )
         {
            Game.MainWindow.RootConsole.SetChar( X, Y, CurrentSymbol.Value );
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
            Symbols = Symbols,
            CurrentSymbol = CurrentSymbol,
            CurrentSymbolIndex = CurrentSymbolIndex
         };
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

   public class LineAnimation
   {
      public Point Origin { get; set; }
      public Point Destination { get; set; }
      public CellAnimation CellAnimation { get; set; }
      public long SpeedMs { get; set; }

      public LineAnimation( Point origin, Point destination, CellAnimation cellAnimation, long speedMs )
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
            AnimationManager.AddAnimation( cellAnimation, ++i * SpeedMs );
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

   public class CircleAnimation
   {
      public Point Center { get; set; }
      public int Radius { get; set; }
      public CellAnimation CellAnimation { get; set; }
      public long SpeedMs { get; set; }

      public CircleAnimation( Point center, int radius, CellAnimation cellAnimation, long speedMs )
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
               AnimationManager.AddAnimation( cellAnimation, ( i * SpeedMs ) + startOffsetMs );
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
}
