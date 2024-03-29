﻿using System;
using RogueSharp.ConsoleEngine;
using RogueSharp.Random;
using RogueSharpRLNetSamples.Core;
using RogueSharpRLNetSamples.Items;
using RogueSharpRLNetSamples.Systems;

namespace RogueSharpRLNetSamples
{
   public static class Game
   {
      private static readonly int _screenWidth = 100;
      private static readonly int _screenHeight = 70;
      private static readonly int _mapWidth = 80;
      private static readonly int _mapHeight = 48;
      private static readonly int _messageWidth = 80;
      private static readonly int _messageHeight = 11;
      private static readonly int _statWidth = 20;
      private static readonly int _statHeight = 70;
      private static readonly int _inventoryWidth = 80;
      private static readonly int _inventoryHeight = 11;

      private static RSWindow _mainWindow;
      private static RSConsole _mapConsole;
      private static RSConsole _messageConsole;
      private static RSConsole _statConsole;
      private static RSConsole _inventoryConsole;

      private static int _mapLevel = 1;
      private static bool _renderRequired = true;

      public static Player Player { get; set; }
      public static DungeonMap DungeonMap { get; private set; }
      public static MessageLog MessageLog { get; private set; }
      public static CommandSystem CommandSystem { get; private set; }
      public static SchedulingSystem SchedulingSystem { get; private set; }
      public static TargetingSystem TargetingSystem { get; private set; }
      public static IRandom Random { get; private set; }

      public static void Main()
      {
         string fontFileName = "terminal8x8.png";
         string consoleTitle = "RougeSharp RLNet Tutorial - Level 1";

         int characterPixelWidth = 8;
         int characterPixelHeight = 8;
         int fontWidthInColumns = 16;
         int fontHeightInRows = 16;

         BitmapFont bitmapFont = new BitmapFont( characterPixelWidth, characterPixelHeight, fontWidthInColumns, fontHeightInRows, fontFileName );

         int seed = (int) DateTime.UtcNow.Ticks;
         Random = new DotNetRandom( seed );

         MessageLog = new MessageLog();
         MessageLog.Add( "The rogue arrives on level 1" );
         MessageLog.Add( $"Level created with seed '{seed}'" );

         Player = new Player();
         SchedulingSystem = new SchedulingSystem();

         MapGenerator mapGenerator = new MapGenerator( _mapWidth, _mapHeight, 20, 13, 7, _mapLevel );
         DungeonMap = mapGenerator.CreateMap();

         _mainWindow = new RSWindow( bitmapFont, _screenWidth, _screenHeight, consoleTitle );
         _mapConsole = new RSConsole( _mapWidth, _mapHeight );
         _messageConsole = new RSConsole( _messageWidth, _messageHeight );
         _statConsole = new RSConsole( _statWidth, _statHeight );
         _inventoryConsole = new RSConsole( _inventoryWidth, _inventoryHeight );

         CommandSystem = new CommandSystem();
         TargetingSystem = new TargetingSystem();

         Player.Item1 = new RevealMapScroll();
         Player.Item2 = new RevealMapScroll();

         _mainWindow.KeyDown += OnMainWindowKeyDown;
         _mainWindow.Update += OnMainWindowUpdate;
         _mainWindow.Render += OnMainWindowRender;
         _mainWindow.Start();
      }

      private static void OnMainWindowKeyDown( object sender, KeyEventArgs e )
      {
         bool didPlayerAct = false;
         if ( e?.Key == null )
         {
            return;
         }
         RSKeyCode keyCodePressed = e.Key.KeyCode;

         if ( TargetingSystem.IsPlayerTargeting )
         {
            if ( keyCodePressed != null )
            {
               _renderRequired = true;
               TargetingSystem.HandleKey( keyCodePressed );
            }
         }
         else if ( CommandSystem.IsPlayerTurn )
         {
            if ( keyCodePressed != null )
            {
               if ( keyCodePressed == RSKeyCode.Up )
               {
                  didPlayerAct = CommandSystem.MovePlayer( Direction.Up );
               }
               else if ( keyCodePressed == RSKeyCode.Down )
               {
                  didPlayerAct = CommandSystem.MovePlayer( Direction.Down );
               }
               else if ( keyCodePressed == RSKeyCode.Left )
               {
                  didPlayerAct = CommandSystem.MovePlayer( Direction.Left );
               }
               else if ( keyCodePressed == RSKeyCode.Right )
               {
                  didPlayerAct = CommandSystem.MovePlayer( Direction.Right );
               }
               else if ( keyCodePressed == RSKeyCode.Escape )
               {
                  _mainWindow.Quit();
               }
               else if ( keyCodePressed == RSKeyCode.Period )
               {
                  if ( DungeonMap.CanMoveDownToNextLevel() )
                  {
                     MapGenerator mapGenerator = new MapGenerator( _mapWidth, _mapHeight, 20, 13, 7, ++_mapLevel );
                     DungeonMap = mapGenerator.CreateMap();
                     MessageLog = new MessageLog();
                     CommandSystem = new CommandSystem();
                     _mainWindow.WindowTitle = $"RougeSharp RLNet Tutorial - Level {_mapLevel}";
                     didPlayerAct = true;
                  }
               }
               else
               {
                  didPlayerAct = CommandSystem.HandleKey( keyCodePressed );
               }

               if ( didPlayerAct )
               {
                  _renderRequired = true;
                  CommandSystem.EndPlayerTurn();
               }
            }
         }
         else
         {
            CommandSystem.ActivateMonsters();
            _renderRequired = true;
         }
      }

      private static void OnMainWindowUpdate( object sender, FrameEventArgs e )
      {
         if ( !TargetingSystem.IsPlayerTargeting && !CommandSystem.IsPlayerTurn )
         {
            CommandSystem.ActivateMonsters();
            _renderRequired = true;
         }
      }

      private static void OnMainWindowRender( object sender, FrameEventArgs e )
      {
         if ( _renderRequired )
         {
            _mapConsole.Clear();
            _messageConsole.Clear();
            _statConsole.Clear();
            _inventoryConsole.Clear();
            DungeonMap.Draw( _mapConsole, _statConsole, _inventoryConsole );
            MessageLog.Draw( _messageConsole );
            TargetingSystem.Draw( _mapConsole );
            RSConsole.Blit( _mapConsole, 0, 0, _mapWidth, _mapHeight, _mainWindow.RootConsole, 0, _inventoryHeight );
            RSConsole.Blit( _statConsole, 0, 0, _statWidth, _statHeight, _mainWindow.RootConsole, _mapWidth, 0 );
            RSConsole.Blit( _messageConsole, 0, 0, _messageWidth, _messageHeight, _mainWindow.RootConsole, 0, _screenHeight - _messageHeight );
            RSConsole.Blit( _inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _mainWindow.RootConsole, 0, 0 );
            _mainWindow.Draw();

            _renderRequired = false;
         }
      }
   }
}
