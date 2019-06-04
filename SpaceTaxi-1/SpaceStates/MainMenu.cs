using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.SpaceStates {
    public class MainMenu : IGameState {
        private static MainMenu instance;
        private int activeMenuButton;

        private Entity backGroundImage;
        private Text[] menuButtons;

        public MainMenu() {
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f),
                    new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "black.png"))); //SpaceTaxiImage.png
            menuButtons = new[] {
                new Text("New Game", new Vec2F(0.4f, 0.35f),
                    new Vec2F(0.3f, 0.3f)),
                new Text("Select Level", new Vec2F(0.4f, 0.25f),
                    new Vec2F(0.3f, 0.3f)),
                new Text("Exit", new Vec2F(0.4f, 0.15f),
                    new Vec2F(0.3f, 0.3f))
            };

            activeMenuButton = 0;
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        /// <summary>
        /// Updates button colors in the Main Menu depending on them being active/inactive.
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
            backGroundImage.RenderEntity();
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
                    if (keyAction == "KEY_PRESS") {
                        GameRunning.GetInstance().PaintTheBoard("short-n-sweet.txt");
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_RUNNING", ""));
                    }

                    break;
                case 1:
                    if (keyAction == "KEY_PRESS") {
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "SELECT_LEVEL", ""));
                    }

                    break;
                case 2:
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW",
                            "", ""));
                    break;
                }

                break;
            }
        }
        
        /// <summary>
        /// Gets an instance of MainMenu.
        /// </summary>
        /// <returns>Returns an instance of MainMenu</returns>   
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }
    }
}