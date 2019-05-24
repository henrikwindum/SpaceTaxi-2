using DIKUArcade.EventBus;

namespace SpaceTaxi_1 {
    public class SpaceBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return SpaceBus.eventBus ?? (SpaceBus.eventBus = new GameEventBus<object>());
        }
    }
}