using System;

namespace SpaceTaxi_1.SpaceStates {
    public class StateTransformer {

        public static GameStateType TransformStringToState(string state) {
            switch (state) {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            case "GAME_OVER":
                return GameStateType.GameOver;
            case "SELECT_LEVEL":
                return GameStateType.SelectLevel;
            default:
                throw new Exception("INVALID STRING");
            }
        }

        public static string TransformStateToString(GameStateType state) {
            switch (state ) {
            case GameStateType.GameRunning:
                return "GAME_RUNNING";
            case GameStateType.GamePaused:
                return "GAME_PAUSED";
            case GameStateType.MainMenu:
                return "MAIN_MENU";
            case GameStateType.GameOver:
                return "GAME_OVER";
            case GameStateType.SelectLevel:
                return "SELECT_LEVEL";
            default:
                throw new Exception("INVALID STATE");
            }
        }
    }
}