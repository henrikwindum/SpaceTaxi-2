using NUnit.Framework;
using SpaceTaxi_1.SpaceStates;

namespace SpaceTaxiStateMachineTesting {
    public class StateTranformerTesting {
        
        /// <summary>
        /// Testing the string "GAME_PAUSED" changes GameStateType to GamePaused. 
        /// </summary>
        [Test]
        public void GamePauseTest1() {
            Assert.AreEqual("GAME_PAUSED",
                StateTransformer.TransformStateToString(GameStateType.GamePaused));
        }
        
        /// <summary>
        /// Testing the GameStateType GamePaused returns the string "GAME_PAUSED".
        /// </summary>
        [Test]
        public void GamePauseTest2() {
            Assert.AreEqual(GameStateType.GamePaused,
                StateTransformer.TransformStringToState("GAME_PAUSED"));
        }

        /// <summary>
        /// Testing the string "GAME_RUNNING" changes GameStateType to GameRunning.
        /// </summary>
        [Test]
        public void GameRunningTest1() {
            Assert.AreEqual("GAME_RUNNING",
                StateTransformer.TransformStateToString(GameStateType.GameRunning));
        }
        
        /// <summary>
        /// Testing the GameStateType MainMenu returns the string "MAIN_MENU".
        /// </summary>
        [Test]
        public void GameRunningTest2() {
            Assert.AreEqual(GameStateType.MainMenu,
                StateTransformer.TransformStringToState("MAIN_MENU"));
        }

        /// <summary>
        /// Testing the string "MAIN_MENU" changes GameStateType to MainMenu.
        /// </summary>
        [Test]
        public void MainMenuTest1() {
            Assert.AreEqual("MAIN_MENU",
                StateTransformer.TransformStateToString(GameStateType.MainMenu));
        }

        /// <summary>
        /// Testing the GameStateType MainMenu returns the string "MAIN_MENU".
        /// </summary>
        [Test]
        public void MainMenuTest2() {
            Assert.AreEqual(GameStateType.MainMenu,
                StateTransformer.TransformStringToState("MAIN_MENU"));
        }

        /// <summary>
        /// Testing the string "GAME_OVER" changes GameStateType to GameOver.
        /// </summary>
        [Test]
        public void GameOverTest1() {
            Assert.AreEqual("GAME_OVER",
                StateTransformer.TransformStateToString(GameStateType.GameOver));
        }

        /// <summary>
        /// Testing the GameStateType GameOver returns the string "GAME_OVER".
        /// </summary>
        [Test]
        public void GameOverTest2() {
            Assert.AreEqual(GameStateType.GameOver,
                StateTransformer.TransformStringToState("GAME_OVER"));
        }

        /// <summary>
        /// Testing the string "SELECT_LEVEL" changes GameStateType to SelectLevel.
        /// </summary>
        [Test]
        public void SelectLevelTest1() {
            Assert.AreEqual("SELECT_LEVEL",
                StateTransformer.TransformStateToString(GameStateType.SelectLevel));
        }

        /// <summary>
        /// Testing the GameStateType SelectLevel returns the string "SELECT_LEVEL".
        /// </summary>
        [Test]
        public void SelectLevelTest2() {
            Assert.AreEqual(GameStateType.SelectLevel,
                StateTransformer.TransformStringToState("SELECT_LEVEL"));
        }
    }
}