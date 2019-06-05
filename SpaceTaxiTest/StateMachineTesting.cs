using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using NUnit.Framework;
using SpaceTaxi_1;
using SpaceTaxi_1.SpaceStates;

namespace SpaceTaxiStateMachineTesting {
    [TestFixture]
    public class StateMachineTesting {
        [SetUp]
        public void InitiateStateMachine() {
            Window.CreateOpenGLContext();

            SpaceBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent,
                GameEventType.InputEvent
            });

            stateMachine = new StateMachine();

            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
        }

        private StateMachine stateMachine;

        /// <summary>
        /// Makes sure the active state becomes GameOver when receiving the message "GAME_OVER".
        /// </summary>
        [Test]
        public void TestEventGameOver() {
            SpaceBus.GetBus()
                .RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "CHANGE_STATE",
                    "GAME_OVER", ""));

            SpaceBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameOver>());
        }

        /// <summary>
        /// Makes sure the active state becomes GamePaused when receiving the message "GAME_PAUSED".
        /// </summary>
        [Test]
        public void TestEventPaused() {
            SpaceBus.GetBus()
                .RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "CHANGE_STATE",
                    "GAME_PAUSED", ""));

            SpaceBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        /// <summary>
        /// Makes sure the active state becomes SelectLevel when receiving the message "SELECT_LEVEL".
        /// </summary>
        [Test]
        public void TestEventSelectLevel() {
            SpaceBus.GetBus()
                .RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent, this, "CHANGE_STATE",
                    "SELECT_LEVEL", ""));

            SpaceBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<SelectLevel>());
        }

        /// <summary>
        /// Tests that the initial state is in fact MainMenu.
        /// </summary>
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
    }
}