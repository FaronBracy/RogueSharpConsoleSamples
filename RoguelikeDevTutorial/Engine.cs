using RogueSharp;
using RogueSharp.ConsoleEngine;

namespace RoguelikeDevTutorial
{
   public class Engine
   {
      public InputHandler InputHandler { get; private set; }
      public Entity Player { get; private set; }
      public GameMap GameMap { get; private set; }
      public FieldOfView<Tile> FieldOfView { get; private set; }

      public Engine( InputHandler inputHandler, Entity player, GameMap gameMap )
      {
         InputHandler = inputHandler;
         Player = player;
         GameMap = gameMap;
         FieldOfView = new FieldOfView<Tile>( gameMap );
         UpdateFieldOfView();
      }

      public void HandleInput( RSKey key )
      {
         IAction action = InputHandler.HandleKey( key, this );
         action.Perform();
         UpdateFieldOfView();
      }

      public void UpdateFieldOfView()
      {
         foreach ( Tile tile in GameMap.GetAllCells() )
         {
            tile.IsVisible = false;
         }
         foreach ( Tile tile in FieldOfView.ComputeFov( Player.X, Player.Y, 8, true ) )
         {
            tile.IsVisible = true;
            tile.IsExplored = true;
         }
      }

      public void Render( RSWindow mainWindow )
      {
         mainWindow.RootConsole.Clear();

         GameMap.Render( mainWindow );  

         mainWindow.Draw();
      }
   }
}
