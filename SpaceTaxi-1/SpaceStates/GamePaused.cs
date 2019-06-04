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
    public class GamePaused : IGameState {
        private static GamePaused instance;
        private int activeMenuButton;
        private Text[] menuButtons;
        private Entity pauseImage;

        public GamePaused() {
            pauseImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f),
                    new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "black.png"))); //SpaceTaxiImage.png
            menuButtons = new[] {
                new Text("Resume", new Vec2F(0.4f, 0.35f),
                    new Vec2F(0.3f, 0.3f)),
                new Text("Main menu", new Vec2F(0.4f, 0.25f),
                    new Vec2F(0.3f, 0.3f)),
                new Text("Quit", new Vec2F(0.4f, 0.15f),
                    new Vec2F(0.3f, 0.3f))
            };

            activeMenuButton = 0;
        }

        public void GameLoop() { }

        public void InitializeGameState() { }
        
        /// <summary>
        /// Updates button colors in the Game Paused menu depending on them being active/inactive.
        /// </summary>
        public void UpdateGameLogic() {
            foreach (var button in menuButtons) {
                button.SetColor(Color.White);
            }

            menuButtons[activeMenuButton].SetColor(Color.Red);
        }
        
        /// <summary>
        /// Renders background and buttons.
        /// </summary>
        public void RenderState() {
            pauseImage.RenderEntity();
            foreach (var button in menuButtons) {
                button.RenderText();
            }
        }

        /// <summary>
        /// Handles key events. Increasing/decreasing the value of activeMenuButton or registering
        /// an event.
        /// </summary>
        /// <param name="keyValue">The pressed/released key.</param>
        /// <param name="keyAction">Information of whether key pressed or released</param>
        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyValue) {
            case "KEY_UP":
                if (keyAction == "KEY_PRESS") {
                    if (activeMenuButton > 0) {
                        activeMenuButton--;
                    }
                }

                break;
            case "KEY_DOWN":
                if (keyAction == "KEY_PRESS") {
                    if (activeMenuButton < menuButtons.Length - 1) {
                        activeMenuButton++;
                    }
                }

                break;
            case "KEY_ENTER":
                switch (activeMenuButton) {
                case 0:
                    StaticTimer.ResumeTimer();
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors
                        (GameEventType.GameStateEvent, this,
                            "CHANGE_STATE", "GAME_RUNNING", ""));
                    break;
                case 1:
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors
                        (GameEventType.GameStateEvent, this,
                            "CHANGE_STATE", "MAIN_MENU", ""));
                    break;
                case 2:
                    if (keyAction == "KEY_PRESS") {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.WindowEvent, this, "CLOSE_WINDOW",
                                "", ""));
                    }

                    break;
                }

                break;
            }
        }
        
        /// <summary>
        /// Gets an instance of GameOver.
        /// </summary>
        /// <returns>Returns an instance of GamePaused</returns>   
        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
    }
}