using System;

namespace SpaceTaxi_1.SpaceStates {
    public class StateTransformer {
        
        /// <summary>
        /// Transforms a given string (ex. "GAME_RUNNING") to a GameStateType.
        /// </summary>
        /// <param name="state">the string to be transformed.</param>
        /// <returns>The GameStateType corresponding to the string (state).</returns>
        /// <exception cref="Exception">Given a invalid string, an exception is thrown.</exception>
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

        /// <summary>
        /// Transform a given GameStateType to a string.
        /// </summary>
        /// <param name="state">The GameStateType to be transformed.</param>
        /// <returns>The string corresponding to the given GameStateType.</returns>
        /// <exception cref="Exception">Given an invalid GameStateType,
        /// an exception is thrown.</exception>
        public static string TransformStateToString(GameStateType state) {
            switch (state) {
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