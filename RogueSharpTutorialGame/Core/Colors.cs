using RogueSharp.ConsoleEngine;

namespace RogueSharpRLNetSamples.Core
{
   public static class Colors
   {
      public static RSColor DoorBackground = Swatch.ComplimentDarkest;
      public static RSColor Door = Swatch.ComplimentLighter;
      public static RSColor DoorBackgroundFov = Swatch.ComplimentDarker;
      public static RSColor DoorFov = Swatch.ComplimentLightest;
      public static RSColor FloorBackground = RSColor.Black;
      public static RSColor Floor = Swatch.AlternateDarkest;
      public static RSColor FloorBackgroundFov = Swatch.DbDark;
      public static RSColor FloorFov = Swatch.Alternate;
      public static RSColor WallBackground = Swatch.SecondaryDarkest;
      public static RSColor Wall = Swatch.Secondary;
      public static RSColor WallBackgroundFov = Swatch.SecondaryDarker;
      public static RSColor WallFov = Swatch.SecondaryLighter;
      public static RSColor GoblinColor = RSColor.Green;
      public static RSColor KoboldColor = new RSColor( 255, 165, 0 );
      public static RSColor OozeColor = new RSColor( 102, 205, 170 );
      public static RSColor Player = Swatch.DbLight;
      public static RSColor InventoryHeading = Swatch.DbLight;
   }

   public static class Swatch
   {
      // http://paletton.com/#uid=73d0u0k5qgb2NnT41jT74c8bJ8X

      public static RSColor PrimaryLightest = new RSColor( 110, 121, 119 );
      public static RSColor PrimaryLighter = new RSColor( 88, 100, 98 );
      public static RSColor Primary = new RSColor( 68, 82, 79 );
      public static RSColor PrimaryDarker = new RSColor( 48, 61, 59 );
      public static RSColor PrimaryDarkest = new RSColor( 29, 45, 42 );

      public static RSColor SecondaryLightest = new RSColor( 116, 120, 126 );
      public static RSColor SecondaryLighter = new RSColor( 93, 97, 105 );
      public static RSColor Secondary = new RSColor( 72, 77, 85 );
      public static RSColor SecondaryDarker = new RSColor( 51, 56, 64 );
      public static RSColor SecondaryDarkest = new RSColor( 31, 38, 47 );

      public static RSColor AlternateLightest = new RSColor( 190, 184, 174 );
      public static RSColor AlternateLighter = new RSColor( 158, 151, 138 );
      public static RSColor Alternate = new RSColor( 129, 121, 107 );
      public static RSColor AlternateDarker = new RSColor( 97, 89, 75 );
      public static RSColor AlternateDarkest = new RSColor( 71, 62, 45 );

      public static RSColor ComplimentLightest = new RSColor( 190, 180, 174 );
      public static RSColor ComplimentLighter = new RSColor( 158, 147, 138 );
      public static RSColor Compliment = new RSColor( 129, 116, 107 );
      public static RSColor ComplimentDarker = new RSColor( 97, 84, 75 );
      public static RSColor ComplimentDarkest = new RSColor( 71, 56, 45 );

      // http://pixeljoint.com/forum/forum_posts.asp?TID=12795

      public static RSColor DbDark = new RSColor( 20, 12, 28 );
      public static RSColor DbOldBlood = new RSColor( 68, 36, 52 );
      public static RSColor DbDeepWater = new RSColor( 48, 52, 109 );
      public static RSColor DbOldStone = new RSColor( 78, 74, 78 );
      public static RSColor DbWood = new RSColor( 133, 76, 48 );
      public static RSColor DbVegetation = new RSColor( 52, 101, 36 );
      public static RSColor DbBlood = new RSColor( 208, 70, 72 );
      public static RSColor DbStone = new RSColor( 117, 113, 97 );
      public static RSColor DbWater = new RSColor( 89, 125, 206 );
      public static RSColor DbBrightWood = new RSColor( 210, 125, 44 );
      public static RSColor DbMetal = new RSColor( 133, 149, 161 );
      public static RSColor DbGrass = new RSColor( 109, 170, 44 );
      public static RSColor DbSkin = new RSColor( 210, 170, 153 );
      public static RSColor DbSky = new RSColor( 109, 194, 202 );
      public static RSColor DbSun = new RSColor( 218, 212, 94 );
      public static RSColor DbLight = new RSColor( 222, 238, 214 );
   }
}