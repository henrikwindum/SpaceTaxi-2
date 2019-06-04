using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace SpaceTaxi_1.SpaceStates {
    public class StateMachine : IGameEventProcessor<object> {
        public StateMachine() {
            SpaceBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            SpaceBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = MainMenu.GetInstance();
        }

        public IGameState ActiveState { get; private set; }

        /// <summary>
        /// Processes events of a given type, changing the active state or calling
        /// handleKeyEvent of the active state
        /// </summary>
        /// <param name="eventType">The identified event.</param>
        /// <param name="gameEvent">The active event that are send between system parts.</param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                case "CHANGE_STATE":
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message, 
                    gameEvent.Parameter1);
            }
        }

        /// <summary>
        /// Sets the active state.
        /// </summary>
        /// <param name="stateType">Current GameStateType.</param>
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                GameRunning.NewGetInstance();
                break;
            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
            case GameStateType.GameOver:
                ActiveState = GameOver.GetInstance();
                break;
            case GameStateType.SelectLevel:
                ActiveState = SelectLevel.GetInstance();
                break;
            }
        }
    }
}