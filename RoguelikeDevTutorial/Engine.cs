using System.Collections.Generic;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Engine
   {
      public List<Entity> Entities { get; private set; }
      public InputHandler InputHandler { get; private set; }
      public Entity Player { get; private set; }
      public GameMap GameMap { get; private set; }

      public Engine( List<Entity> entities, InputHandler inputHandler, Entity player, GameMap gameMap )
      {
         Entities = entities;
         InputHandler = inputHandler;
         Player = player;
         GameMap = gameMap;
      }

      public void HandleInput( RSKey key )
      {
         IAction action = InputHandler.HandleKey( key, this );
         action.Execute();
      }

      public void Render( RSWindow mainWindow )
      {
         mainWindow.RootConsole.Clear();

         GameMap.Render( mainWindow );  

         foreach ( Entity entity in Entities )
         {
            mainWindow.RootConsole.Set( entity.X, entity.Y, entity.Color, RSColor.Black, entity.Character );
         }

         mainWindow.Draw();
      }
   }
}
