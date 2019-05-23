using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Timers;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.SpaceStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private Entity backGroundImage;
        private Player player;
        private List<Player> playerList = new List<Player>();
        private PaintBoard paintBoard;
        private Collision collision;
        /*
        private Text scoreText;
        private Text timerText;
        private Text namePlatformText;
        private Entity textPlate;
        */
        private Textplate textPlate;
        public GameRunning() {
            player = new Player();
            player.SetPosition(0.45f, 0.6f);                        

            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            paintBoard = new PaintBoard();            
            collision = new Collision();
            collision.playerList.Add(player);
            StaticTimer.RestartTimer();
            textPlate = new Textplate();
            
            /*
            textPlate = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 0.05f)), 
                new Image(Path.Combine("Assets", "Images", "white-square.png")));
            scoreText = new Text("Score: " + player.score, 
                new Vec2F(0.1f, -0.165f), new Vec2F(0.2f, 0.2f));
            scoreText.SetColor(Color.Red);
            timerText = new Text("Timer: " + StaticTimer.GetElapsedSeconds(),
                new Vec2F(0.25f, -0.165f), new Vec2F(0.2f, 0.2f));
            timerText.SetColor(Color.Red);
            namePlatformText = new Text(player.currentCustomer.name + " | " + player.currentCustomer.platform,
                new Vec2F(0.4f, -0.165f), new Vec2F(0.2f, 0.2f));
            namePlatformText.SetColor(Color.Red);
            */
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() {
            player.Movement();            
            collision.WallCollision(paintBoard.Walls, player);
            collision.PlatformCollision(paintBoard.PlatformList, player);
            paintBoard.CustomerList = collision.CustomerCollision(paintBoard.CustomerList, player);
            player.Entity.Shape.Move();
            player.Gravity();
            NextLevel();            
            foreach (Customer _customer in paintBoard.CustomerList) {
                _customer.Move();
            }
            /*
            scoreText.SetText("Score: " + player.score);
            timerText.SetText("Timer: " + StaticTimer.GetElapsedSeconds());
            namePlatformText.SetText(player.currentCustomer.name + " | " + player.currentCustomer.platform);
            */
            textPlate.Render(player);
            player.sumFunc();            
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            
            foreach (Player _player in collision.playerList) {
                _player.RenderPlayer();
            }

            foreach (Customer _customer in paintBoard.CustomerList) {
                if (_customer.spawnTime < StaticTimer.GetElapsedSeconds()) {
                    _customer.Entity.Shape.Extent = new Vec2F(0.05f, 0.05f);        
                }
                _customer.Render();                             
            }

            foreach (var _platform in paintBoard.PlatformList) {
                _platform.Entity.RenderEntity();
            }            
            paintBoard.Walls.RenderEntities();
            collision.explosion.RenderAnimations();
            textPlate.Render(player);
            /*
            textPlate.RenderEntity();
            scoreText.RenderText();   
            timerText.RenderText();
            */
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            case "KEY_P":
                SpaceBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors
                    (GameEventType.GameStateEvent, this,
                        "CHANGE_STATE", "GAME_PAUSED", ""));
                break;
            }
        }

        public void PaintTheBoard(string level) {
            paintBoard.PaintTheBoard(level);            
        }

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        public static GameRunning NewGetInstance() {
            return GameRunning.instance = new GameRunning();
        }        

        private void NextLevel() {
            if (player.Entity.Shape.Position.Y >= 0.99f) {
                paintBoard.CustomerList.Clear();
                paintBoard.PaintTheBoard("the-beach.txt");
                player.SetPosition(0.3f, 0.5f);
                //player.Entity.Shape.AsDynamicShape().Direction.Y = 0f;                
            }
        }
        
    }
}