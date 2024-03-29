﻿using RogueSharp.ConsoleEngine;
using RogueSharp;
using RogueSharpRLNetSamples.Interfaces;

namespace RogueSharpRLNetSamples.Core
{
   public class Item : IItem, ITreasure, IDrawable
   {
      public Item()
      {
         Symbol = '!';
         Color = RSColor.Yellow;
      }

      public string Name { get; protected set; }
      public int RemainingUses { get; protected set; }

      public bool Use()
      {
         return UseItem();
      }

      protected virtual bool UseItem()
      {
         return false;
      }

      public bool PickUp( IActor actor )
      {
         Player player = actor as Player;

         if ( player != null )
         {
            if ( player.AddItem( this ) )
            {
               Game.MessageLog.Add( $"{actor.Name} picked up {Name}" );
               return true;
            }
         }

         return false;
      }

      public RSColor Color { get; set; }

      public char Symbol { get; set; }

      public int X { get; set; }

      public int Y { get; set; }

      public void Draw( RSConsole console, DungeonMap map )
      {
         if ( !map.IsExplored( X, Y ) )
         {
            return;
         }

         if ( map.IsInFov( X, Y ) )
         {
            console.Set( X, Y, Color, Colors.FloorBackgroundFov, Symbol );
         }
         else
         {
            console.Set( X, Y, RSColor.Blend( Color, RSColor.Gray, 0.5f ), Colors.FloorBackground, Symbol );
         }
      }
   }
}
