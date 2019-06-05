using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Timers;
using SpaceTaxi_1.Taxi;

namespace SpaceTaxi_1.SpaceStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Collision collision;
        private PaintBoard paintBoard;
        private Player player;
        private List<Player> playerList = new List<Player>();
        private Textplate textPlate;

        public GameRunning() {
            player = new Player();
            player.SetPosition(0.45f, 0.6f);

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), 
                    new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            paintBoard = new PaintBoard();
            collision = new Collision();
            collision.PlayerList.Add(player);
            StaticTimer.RestartTimer();
            textPlate = new Textplate();
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        /// <summary>
        /// Updates player movement, gravity, textPlate and customer movement, checks for failed
        /// deliveries, collisions of all types and if the player should enter the next level.
        /// </summary>
        public void UpdateGameLogic() {
            player.Movement();
            player.Entity.Shape.Move();
            player.Gravity();
            player.FailedDelivery();
            collision.WallCollision(paintBoard.Walls, player);
            collision.PlatformCollision(paintBoard.PlatformList, player);
            paintBoard.Customers = collision.CustomerCollision(paintBoard.Customers, player);
            NextLevel();
            textPlate.Update(player);
            foreach (var customer in paintBoard.Customers) {
                customer.Move();
            }
        }

        /// <summary>
        /// Renders the in-game background image, player, customers, platforms, walls,
        /// explosions and the text-plate. 
        /// </summary>
        public void RenderState() {
            backGroundImage.RenderEntity();

            foreach (var tempPlayer in collision.PlayerList) {
                tempPlayer.RenderPlayer();
            }

            foreach (var customer in paintBoard.Customers) {
                customer.Render();
            }

            paintBoard.PlatformList.RenderEntities();
            foreach (Platform platform in paintBoard.PlatformList) {
                platform.Name.RenderText();
            }
            paintBoard.Walls.RenderEntities();
            collision.Explosion.RenderAnimations();
            textPlate.Render();
        }
        
        /// <summary>
        /// Handles key events. When 'p' is pressed the timer is paused and the "GAME_PAUSED" event
        /// is registered.
        /// </summary>
        /// <param name="keyValue">The pressed/released key.</param>
        /// <param name="keyAction">Information of whether key pressed or released.</param>
        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            case "KEY_P":
                StaticTimer.PauseTimer();
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors
                    (GameEventType.GameStateEvent, this,
                        "CHANGE_STATE", "GAME_PAUSED", ""));
                break;
            }
        }
        
        /// <summary>
        /// Paints the board of a given level.
        /// </summary>
        /// <param name="level">Filename of the level.</param>
        public void PaintTheBoard(string level) {
            paintBoard.PaintTheBoard(level);
        }
        
        /// <summary>
        /// Gets an instance of GameRunning.
        /// </summary>
        /// <returns>Returns an instance of GameRunning.</returns>   
        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }
        
        /// <summary>
        /// Gets a new instance of GameRunning.
        /// </summary>
        /// <returns>Returns a new instance of GameRunning.</returns>   
        public static GameRunning NewGetInstance() {
            return GameRunning.instance = new GameRunning();
        }
        
        /// <summary>
        /// Checks if the players position is higher than 0.99f, and if so, clears the current
        /// list of customers, paints a new map and sets the player position.
        /// </summary>
        private void NextLevel() {
            if (player.Entity.Shape.Position.Y >= 0.99f) {                
                paintBoard.Customers.Clear();
                paintBoard.PaintTheBoard("the-beach.txt");
                player.SetPosition(0.3f, 0.5f);
            }
        }
    }
}