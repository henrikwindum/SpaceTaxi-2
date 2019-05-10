using NUnit.Framework;
using SpaceTaxi_1.SpaceStates;

namespace SpaceTaxiStateMachineTesting {
    public class StateTranformerTesting {
        [Test]
        public void GamePauseTest1() {
            Assert.AreEqual("GAME_PAUSED",
                StateTransformer.TransformStateToString(GameStateType.GamePaused));
        }

        [Test]
        public void GamePauseTest2() {
            Assert.AreEqual(GameStateType.GamePaused,
                StateTransformer.TransformStringToState("GAME_PAUSED"));
        }

        [Test]
        public void GameRunningTest1() {
            Assert.AreEqual("GAME_RUNNING",
                StateTransformer.TransformStateToString(GameStateType.GameRunning));
        }

        [Test]
        public void GameRunningTest2() {
            Assert.AreEqual(GameStateType.MainMenu,
                StateTransformer.TransformStringToState("MAIN_MENU"));
        }

        [Test]
        public void MainMenuTest1() {
            Assert.AreEqual("MAIN_MENU",
                StateTransformer.TransformStateToString(GameStateType.MainMenu));
        }

        [Test]
        public void MainMenuTest2() {
            Assert.AreEqual(GameStateType.MainMenu,
                StateTransformer.TransformStringToState("MAIN_MENU"));
        }

        [Test]
        public void GameOverTest1() {
            Assert.AreEqual("GAME_OVER",
                StateTransformer.TransformStateToString(GameStateType.GameOver));
        }

        [Test]
        public void GameOverTest2() {
            Assert.AreEqual(GameStateType.GameOver,
                StateTransformer.TransformStringToState("GAME_OVER"));
        }

        [Test]
        public void SelectLevelTest1() {
            Assert.AreEqual("SELECT_LEVEL",
                StateTransformer.TransformStateToString(GameStateType.SelectLevel));
        }

        [Test]
        public void SelectLevelTest2() {
            Assert.AreEqual(GameStateType.SelectLevel,
                StateTransformer.TransformStringToState("SELECT_LEVEL"));
        }
    }
}