using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1.SpaceStates {
    public class SelectLevel : IGameState {
        private static SelectLevel instance;
        private int activeMenuButton;
        private Entity levelSelectImage;
        private Text[] menuButtons;

        public SelectLevel() {
            levelSelectImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f),
                    new Vec2F(1.0f, 1.0f)),
                new Image(
                    Path.Combine("Assets", "Images", "black.png"))); //SpaceTaxiImage.png
            menuButtons = new[] {
                new Text("Level 1", new Vec2F(0.4f, 0.35f),
                    new Vec2F(0.3f, 0.3f)),
                new Text("Level 2", new Vec2F(0.4f, 0.25f),
                    new Vec2F(0.3f, 0.3f)),
                new Text("Main Menu", new Vec2F(0.4f, 0.15f),
                    new Vec2F(0.3f, 0.3f))
            };
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() {
            foreach (var button in menuButtons) {
                button.SetColor(Color.White);
            }

            menuButtons[activeMenuButton].SetColor(Color.Red);
        }

        public void RenderState() {
            levelSelectImage.RenderEntity();
            foreach (var button in menuButtons) {
                button.RenderText();
            }
        }

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
                    if (activeMenuButton < 2) {
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
                        GameRunning.GetInstance().PaintTheBoard("the-beach.txt");
                        SpaceBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors
                            (GameEventType.GameStateEvent, this,
                                "CHANGE_STATE", "GAME_RUNNING", ""));
                    }

                    break;
                case 2:
                    SpaceBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors
                        (GameEventType.GameStateEvent, this,
                            "CHANGE_STATE", "MAIN_MENU", ""));
                    break;
                }

                break;
            }
        }

        public static SelectLevel GetInstance() {
            return SelectLevel.instance ?? (SelectLevel.instance = new SelectLevel());
        }
    }
}